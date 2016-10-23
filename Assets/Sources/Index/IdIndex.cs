using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System;

public class IdIndex
{
    Group observedCollection;
    Dictionary<int, Entity> lookup = new Dictionary<int, Entity>();

    public IdIndex(Pool pool, IMatcher matcher)
    {
        observedCollection = pool.GetGroup(matcher);

        observedCollection.OnEntityAdded += AddEntity;
        observedCollection.OnEntityRemoved += RemoveEntity;
    }

    ~IdIndex()
    {
        Cleanup();
    }

    public void Cleanup()
    {
        observedCollection.OnEntityAdded -= AddEntity;
        observedCollection.OnEntityRemoved -= RemoveEntity;
        lookup.Clear();
    }


    public Entity FindEntityAtIndex(int id)
    {
        if (lookup.ContainsKey(id))
            return lookup[id];
        return null;
    }

    protected virtual void AddEntity(Group collection, Entity entity, int index, IComponent component)
    {
        IdComponent iDComponent = null;

        foreach (var c in entity.GetComponents())
        {
            if (c.GetType() == typeof(IdComponent))
                iDComponent = (IdComponent)c;
        }

        if (iDComponent != null)
        {
            if (lookup.ContainsKey(iDComponent.Id) && lookup[iDComponent.Id] == entity)
            {
                return;
            }

            if (lookup.ContainsKey(iDComponent.Id) && lookup[iDComponent.Id] != entity)
            {
                throw new Exception("the key " + iDComponent + " is not unique. Present on entity: " + entity.creationIndex + " and entity: " + lookup[iDComponent.Id].creationIndex);
            }
            entity.Retain(this);
            lookup[iDComponent.Id] = entity;
        }

    }

    protected virtual void RemoveEntity(Group collection, Entity entity, int index, IComponent component)
    {
        var iDComponent = component as IdComponent;
        if (iDComponent != null && lookup.ContainsKey(iDComponent.Id))
        {
            lookup[iDComponent.Id].Release(this);
            lookup.Remove(iDComponent.Id);
        }
    }
}