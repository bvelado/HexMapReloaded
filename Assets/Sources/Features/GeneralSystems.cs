using System;
using System.Collections.Generic;
using Entitas;

public class NotifySelectedListeners : IReactiveSystem
{
    Group selectedListeners;

    public NotifySelectedListeners(Pool observedPool)
    {
        selectedListeners = observedPool.GetGroup(Matcher.AnyOf(ViewMatcher.SelectedListener, UIMatcher.SelectedListener));
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Selected).OnEntityAddedOrRemoved();
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
}
