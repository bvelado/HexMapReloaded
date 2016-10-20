using System;
using System.Collections.Generic;

namespace Entitas {

    /// A pool manages the lifecycle of entities and groups.
    /// You can create and destroy entities and get groups of entities.
    /// The prefered way is to use the generated methods from the code generator
    /// to create a Pool, e.g. Pools.sharedInstance.pool = Pools.CreatePool();
    public partial class Pool {

        /// Occurs when an entity gets created.
        public event PoolChanged OnEntityCreated;

        /// Occurs when an entity will be destroyed.
        public event PoolChanged OnEntityWillBeDestroyed;

        /// Occurs when an entity got destroyed.
        public event PoolChanged OnEntityDestroyed;

        /// Occurs when a group gets created for the first time.
        public event GroupChanged OnGroupCreated;

        /// Occurs when a group gets cleared.
        public event GroupChanged OnGroupCleared;

        public delegate void PoolChanged(Pool pool, Entity entity);
        public delegate void GroupChanged(Pool pool, Group group);

        /// The total amount of components an entity can possibly have.
        /// This value is generated by the code generator,
        /// e.g ComponentIds.TotalComponents.
        public int totalComponents { get { return _totalComponents; } }

        /// Returns all componentPools. componentPools is used to reuse
        /// removed components.
        /// Removed components will be pushed to the componentPool.
        /// Use entity.CreateComponent(index, type) to get a new or reusable
        /// component from the componentPool.
        public Stack<IComponent>[] componentPools {
            get { return _componentPools; }
        }

        /// The metaData contains information about the pool.
        /// It's used to provide better error messages.
        public PoolMetaData metaData { get { return _metaData; } }

        /// Returns the number of entities in the pool.
        public int count { get { return _entities.Count; } }

        /// Returns the number of entities in the internal ObjectPool
        /// for entities which can be reused.
        public int reusableEntitiesCount {
            get { return _reusableEntities.Count; }
        }

        /// Returns the number of entities that are currently retained by
        /// other objects (e.g. Group, GroupObserver, ReactiveSystem).
        public int retainedEntitiesCount {
            get { return _retainedEntities.Count; }
        }

        readonly int _totalComponents;
        int _creationIndex;

        readonly HashSet<Entity> _entities = new HashSet<Entity>(
            EntityEqualityComparer.comparer
        );

        readonly Stack<Entity> _reusableEntities = new Stack<Entity>();
        readonly HashSet<Entity> _retainedEntities = new HashSet<Entity>(
            EntityEqualityComparer.comparer
        );

        Entity[] _entitiesCache;

        readonly PoolMetaData _metaData;

        readonly Dictionary<IMatcher, Group> _groups =
            new Dictionary<IMatcher, Group>();

        readonly List<Group>[] _groupsForIndex;

        readonly Stack<IComponent>[] _componentPools;
        readonly Dictionary<string, IEntityIndex> _entityIndices;

        // Cache delegates to avoid gc allocations
        Entity.EntityChanged _cachedEntityChanged;
        Entity.ComponentReplaced _cachedComponentReplaced;
        Entity.EntityReleased _cachedEntityReleased;

        /// The prefered way is to use the generated methods from the
        /// code generator to create a Pool,
        /// e.g. Pools.sharedInstance.pool = Pools.CreatePool();
        public Pool(int totalComponents) : this(totalComponents, 0, null) {
        }

        /// The prefered way is to use the generated methods from the
        /// code generator to create a Pool,
        /// e.g. Pools.sharedInstance.pool = Pools.CreatePool();
        public Pool(int totalComponents,
                    int startCreationIndex,
                    PoolMetaData metaData) {
            _totalComponents = totalComponents;
            _creationIndex = startCreationIndex;

            if(metaData != null) {
                _metaData = metaData;

                if(metaData.componentNames.Length != totalComponents) {
                    throw new PoolMetaDataException(this, metaData);
                }
            } else {

                // If Pools.CreatePool() was used to create the pool,
                // we will never end up here.
                // This is a fallback when the pool is created manually.

                var componentNames = new string[totalComponents];
                const string prefix = "Index ";
                for (int i = 0; i < componentNames.Length; i++) {
                    componentNames[i] = prefix + i;
                }
                _metaData = new PoolMetaData(
                    "Unnamed Pool", componentNames, null
                );
            }

            _groupsForIndex = new List<Group>[totalComponents];
            _componentPools = new Stack<IComponent>[totalComponents];
            _entityIndices = new Dictionary<string, IEntityIndex>();

            // Cache delegates to avoid gc allocations
            _cachedEntityChanged = updateGroupsComponentAddedOrRemoved;
            _cachedComponentReplaced = updateGroupsComponentReplaced;
            _cachedEntityReleased = onEntityReleased;
        }

        /// Creates a new entity or gets a reusable entity from the
        /// internal ObjectPool for entities.
        public virtual Entity CreateEntity() {
            var entity = _reusableEntities.Count > 0
                    ? _reusableEntities.Pop()
                    : new Entity( _totalComponents, _componentPools, _metaData);
            entity._isEnabled = true;
            entity._creationIndex = _creationIndex++;
            entity.Retain(this);
            _entities.Add(entity);
            _entitiesCache = null;
            entity.OnComponentAdded +=_cachedEntityChanged;
            entity.OnComponentRemoved += _cachedEntityChanged;
            entity.OnComponentReplaced += _cachedComponentReplaced;
            entity.OnEntityReleased += _cachedEntityReleased;

            if(OnEntityCreated != null) {
                OnEntityCreated(this, entity);
            }

            return entity;
        }

        /// Destroys the entity, removes all its components and pushs it back
        /// to the internal ObjectPool for entities.
        public virtual void DestroyEntity(Entity entity) {
            var removed = _entities.Remove(entity);
            if(!removed) {
                throw new PoolDoesNotContainEntityException(
                    "'" + this + "' cannot destroy " + entity + "!",
                    "Did you call pool.DestroyEntity() on a wrong pool?"
                );
            }
            _entitiesCache = null;

            if(OnEntityWillBeDestroyed != null) {
                OnEntityWillBeDestroyed(this, entity);
            }

            entity.destroy();

            if(OnEntityDestroyed != null) {
                OnEntityDestroyed(this, entity);
            }

            if(entity.retainCount == 1) {
                // Can be released immediately without
                // adding to _retainedEntities
                entity.OnEntityReleased -= _cachedEntityReleased;
                _reusableEntities.Push(entity);
                entity.Release(this);
                entity.removeAllOnEntityReleasedHandlers();
            } else {
                _retainedEntities.Add(entity);
                entity.Release(this);
            }
        }

        /// Destroys all entities in the pool.
        /// Throws an exception if there are still retained entities.
        public virtual void DestroyAllEntities() {
            var entities = GetEntities();
            for (int i = 0; i < entities.Length; i++) {
                DestroyEntity(entities[i]);
            }

            _entities.Clear();

            if(_retainedEntities.Count != 0) {
                throw new PoolStillHasRetainedEntitiesException(this);
            }
        }

        /// Determines whether the pool has the specified entity.
        public virtual bool HasEntity(Entity entity) {
            return _entities.Contains(entity);
        }

        /// Returns all entities which are currently in the pool.
        public virtual Entity[] GetEntities() {
            if(_entitiesCache == null) {
                _entitiesCache = new Entity[_entities.Count];
                _entities.CopyTo(_entitiesCache);
            }

            return _entitiesCache;
        }

        /// Returns a group for the specified matcher.
        /// Calling pool.GetGroup(matcher) with the same matcher will always
        /// return the same instance of the group.
        public virtual Group GetGroup(IMatcher matcher) {
            Group group;
            if(!_groups.TryGetValue(matcher, out group)) {
                group = new Group(matcher);
                var entities = GetEntities();
                for (int i = 0; i < entities.Length; i++) {
                    group.HandleEntitySilently(entities[i]);
                }
                _groups.Add(matcher, group);

                for (int i = 0; i < matcher.indices.Length; i++) {
                    var index = matcher.indices[i];
                    if(_groupsForIndex[index] == null) {
                        _groupsForIndex[index] = new List<Group>();
                    }
                    _groupsForIndex[index].Add(group);
                }

                if(OnGroupCreated != null) {
                    OnGroupCreated(this, group);
                }
            }

            return group;
        }

        /// Clears all groups. This is useful when you want to
        /// soft-restart your application.
        public void ClearGroups() {
            foreach(var group in _groups.Values) {
                group.RemoveAllEventHandlers();
                var entities = group.GetEntities();
                for (int i = 0; i < entities.Length; i++) {
                    entities[i].Release(group);
                }

                if(OnGroupCleared != null) {
                    OnGroupCleared(this, group);
                }
            }
            _groups.Clear();

            for (int i = 0; i < _groupsForIndex.Length; i++) {
                _groupsForIndex[i] = null;
            }
        }

        /// Adds the IEntityIndex for the specified name.
        /// There can only be one IEntityIndex per name.
        public void AddEntityIndex(string name, IEntityIndex entityIndex) {
            if(_entityIndices.ContainsKey(name)) {
                throw new PoolEntityIndexDoesAlreadyExistException(this, name);
            }

            _entityIndices.Add(name, entityIndex);
        }

        /// Gets the IEntityIndex for the specified name.
        public IEntityIndex GetEntityIndex(string name) {
            IEntityIndex entityIndex;
            if(!_entityIndices.TryGetValue(name, out entityIndex)) {
                throw new PoolEntityIndexDoesNotExistException(this, name);
            }

            return entityIndex;
        }

        /// Deactivates and removes all entity indices.
        public void DeactivateAndRemoveEntityIndices() {
            foreach(var entityIndex in _entityIndices.Values) {
                entityIndex.Deactivate();
            }

            _entityIndices.Clear();
        }

        /// Resets the creationIndex back to 0.
        public void ResetCreationIndex() {
            _creationIndex = 0;
        }

        /// Clears the componentPool at the specified index.
        public void ClearComponentPool(int index) {
            var componentPool = _componentPools[index];
            if(componentPool != null) {
                componentPool.Clear();
            }
        }

        /// Clears all componentPools.
        public void ClearComponentPools() {
            for (int i = 0; i < _componentPools.Length; i++) {
                ClearComponentPool(i);
            }
        }

        /// Resets the pool (clears all groups, destroys all entities and
        /// resets creationIndex back to 0).
        public void Reset() {
            ClearGroups();
            DestroyAllEntities();
            ResetCreationIndex();

            OnEntityCreated = null;
            OnEntityWillBeDestroyed = null;
            OnEntityDestroyed = null;
            OnGroupCreated = null;
            OnGroupCleared = null;
        }

        public override string ToString() {
            return _metaData.poolName;
        }

        void updateGroupsComponentAddedOrRemoved(
            Entity entity, int index, IComponent component) {
            var groups = _groupsForIndex[index];
            if(groups != null) {
                var events = EntitasCache.GetGroupChangedList();

                    for(int i = 0; i < groups.Count; i++) {
                        events.Add(groups[i].handleEntity(entity));
                    }

                    for(int i = 0; i < events.Count; i++) {
                        var groupChangedEvent = events[i];
                        if(groupChangedEvent != null) {
                            groupChangedEvent(
                                groups[i], entity, index, component
                            );
                        }
                    }

                EntitasCache.PushGroupChangedList(events);
            }
        }

        void updateGroupsComponentReplaced(Entity entity,
                                           int index,
                                           IComponent previousComponent,
                                           IComponent newComponent) {
            var groups = _groupsForIndex[index];
            if(groups != null) {
                for (int i = 0; i < groups.Count; i++) {
                    groups[i].UpdateEntity(
                        entity, index, previousComponent, newComponent
                    );
                }
            }
        }

        void onEntityReleased(Entity entity) {
            if(entity._isEnabled) {
                throw new EntityIsNotDestroyedException(
                    "Cannot release " + entity + "!"
                );
            }
            entity.removeAllOnEntityReleasedHandlers();
            _retainedEntities.Remove(entity);
            _reusableEntities.Push(entity);
        }
    }

    public class PoolDoesNotContainEntityException : EntitasException {
        public PoolDoesNotContainEntityException(string message, string hint) :
            base(message + "\nPool does not contain entity!", hint) {
        }
    }

    public class EntityIsNotDestroyedException : EntitasException {
        public EntityIsNotDestroyedException(string message) :
            base(message + "\nEntity is not destroyed yet!",
                "Did you manually call entity.Release(pool) yourself? " +
                "If so, please don't :)") {
        }
    }

    public class PoolStillHasRetainedEntitiesException : EntitasException {
        public PoolStillHasRetainedEntitiesException(Pool pool) : base(
            "'" + pool + "' detected retained entities " +
            "although all entities got destroyed!",
            "Did you release all entities? Try calling pool.ClearGroups() " +
            "and systems.ClearReactiveSystems() before calling " +
            "pool.DestroyAllEntities() to avoid memory leaks.") {
        }
    }

    public class PoolMetaDataException : EntitasException {
        public PoolMetaDataException(Pool pool, PoolMetaData poolMetaData) :
            base("Invalid PoolMetaData for '" + pool + "'!\nExpected " +
                 pool.totalComponents + " componentName(s) but got " +
                 poolMetaData.componentNames.Length + ":",
                 string.Join("\n", poolMetaData.componentNames)) {
        }
    }

    public class PoolEntityIndexDoesNotExistException : EntitasException {
        public PoolEntityIndexDoesNotExistException(Pool pool, string name) :
            base("Cannot get EntityIndex '" + name + "' from pool '" +
                 pool + "'!", "No EntityIndex with this name has been added.") {
        }
    }

    public class PoolEntityIndexDoesAlreadyExistException : EntitasException {
        public PoolEntityIndexDoesAlreadyExistException(Pool pool, string name) :
            base("Cannot add EntityIndex '" + name + "' to pool '" + pool + "'!",
                 "An EntityIndex with this name has already been added.") {
        }
    }

    /// The metaData contains information about the pool.
    /// It's used to provide better error messages.
    public class PoolMetaData {

        public readonly string poolName;
        public readonly string[] componentNames;
        public readonly Type[] componentTypes;

        public PoolMetaData(string poolName,
                            string[] componentNames,
                            Type[] componentTypes) {
            this.poolName = poolName;
            this.componentNames = componentNames;
            this.componentTypes = componentTypes;
        }
    }
}
