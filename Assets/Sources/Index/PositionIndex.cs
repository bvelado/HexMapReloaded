using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System;

public class PositionIndex
{
    Group observedCollection;
    Dictionary<Vector3, Entity> lookup = new Dictionary<Vector3, Entity>();

    public PositionIndex(Pool pool, IMatcher matcher)
    {
        observedCollection = pool.GetGroup(matcher);

        observedCollection.OnEntityAdded += AddEntity;
        observedCollection.OnEntityRemoved += RemoveEntity;
    }

    ~PositionIndex()
    {
        Cleanup();
    }

    public void Cleanup()
    {
        observedCollection.OnEntityAdded -= AddEntity;
        observedCollection.OnEntityRemoved -= RemoveEntity;
        lookup.Clear();
    }


    public Entity FindEntityAtMapPosition(Vector3 mapPosition)
    {
        return lookup[mapPosition];
    }

    protected virtual void AddEntity(Group collection, Entity entity, int index, IComponent component)
    {
        MapPositionComponent mapPositionComponent = null;
        
        foreach(var c in entity.GetComponents())
        {
            if (c.GetType() == typeof(MapPositionComponent))
                mapPositionComponent = (MapPositionComponent)c;
        }

        if (mapPositionComponent != null)
        {
            if (lookup.ContainsKey(mapPositionComponent.Position) && lookup[mapPositionComponent.Position] == entity)
            {
                return;
            }

            if (lookup.ContainsKey(mapPositionComponent.Position) && lookup[mapPositionComponent.Position] != entity)
            {
                throw new Exception("the key " + mapPositionComponent + " is not unique. Present on entity: " + entity.creationIndex + " and entity: " + lookup[mapPositionComponent.Position].creationIndex);
            }
            entity.Retain(this);
            lookup[mapPositionComponent.Position] = entity;
        }

    }

    protected virtual void RemoveEntity(Group collection, Entity entity, int index, IComponent component)
    {
        var mapPositionComponent = component as MapPositionComponent;
        if (mapPositionComponent != null && lookup.ContainsKey(mapPositionComponent.Position))
        {
            lookup[mapPositionComponent.Position].Release(this);
            lookup.Remove(mapPositionComponent.Position);
        }
    }
}