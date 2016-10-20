using System;
using System.Collections.Generic;
using Entitas;

public class NotifySelectedListeners : IReactiveSystem, ISetPool
{
    Group selectedListeners;

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Selected).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            foreach (var l in selectedListeners.GetEntities())
            {
                l.selectedListener.Listener.SelectedChanged(e);
            }
        }
    }

    public void SetPool(Pool pool)
    {
        selectedListeners = Pools.sharedInstance.view.GetGroup(Matcher.AllOf(ViewMatcher.SelectedListener));
    }
}
