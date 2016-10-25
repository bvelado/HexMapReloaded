using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class NotifySelectedListenersSystem : IMultiReactiveSystem
{
    Group selectedListeners;

    public NotifySelectedListenersSystem(Pool observedPool)
    {
        selectedListeners = observedPool.GetGroup(Matcher.AnyOf(ViewMatcher.SelectedListener, UIMatcher.SelectedListener));
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Selected).OnEntityAdded();
        }
    }

    public TriggerOnEvent[] triggers
    {
        get
        {
            return new []
            {
               Matcher.AllOf(CoreMatcher.Selected).OnEntityAdded(),
               Matcher.AllOf(CoreMatcher.Selected).OnEntityRemoved()
            };
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var l in selectedListeners.GetEntities())
        {
            l.selectedListener.Listener.SelectedChanged(Pools.sharedInstance.core.selectedEntity);
        }

        //Debug.Log(entities.Count);
        //if(entities.Count > 0)
        //{
        //    foreach (var e in entities)
        //    {
        //        foreach (var l in selectedListeners.GetEntities())
        //        {
        //            l.selectedListener.Listener.SelectedChanged(e);
        //        }
        //    }
        //} else {
        //    foreach (var l in selectedListeners.GetEntities())
        //    {
        //        Debug.Log("Pas de selectedEntity");
        //        l.selectedListener.Listener.SelectedChanged(null);
        //    }
        //}
    }
}
