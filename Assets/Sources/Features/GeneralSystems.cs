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

public class NotifyActionModeChangedListenersSystem : IReactiveSystem
{
    Group listeners;

    public NotifyActionModeChangedListenersSystem(Pool pool)
    {
        listeners = pool.GetGroup(Matcher.AllOf(CoreMatcher.ActionModeChangedListener, ViewMatcher.ActionModeChangedListener, UIMatcher.ActionModeChangedListener));
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return ParametersMatcher.ActionMode.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            foreach(var l in listeners.GetEntities())
            {
                l.actionModeChangedListener.Listener.ActionModeChanged(e.actionMode.Mode);
            }
        }
    }
}

public class ResetSelectedOnActionModeChanged : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return ParametersMatcher.ActionMode.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            if (Pools.sharedInstance.core.isSelected)
                Pools.sharedInstance.core.selectedEntity.IsSelected(false);
        }
    }
}

public class ActivateSystemsOnActionMode : IExecuteSystem
{
    bool execute;
    ActionMode _mode;
    Systems _systems;

    public ActivateSystemsOnActionMode(ActionMode mode, Systems systems)
    {
        _mode = mode;
        _systems = systems;

        Pools.sharedInstance.parameters.GetGroup(ParametersMatcher.ActionMode);
    }

    public void Execute()
    {
        if (execute)
        {
            Debug.Log(_systems.ToString() + " executing");
            _systems.Execute();
            _systems.Cleanup();
        }   
    }
}