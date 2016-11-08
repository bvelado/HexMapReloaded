using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DestroySystem : IReactiveSystem, ISetPool
{
    Pool _pool;

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AnyOf(CoreMatcher.Destroy, ViewMatcher.Destroy, UIMatcher.Destroy).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            _pool.DestroyEntity(e);   
        }
    }

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
}

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
        if (Pools.sharedInstance.core.isSelected)
            Pools.sharedInstance.core.selectedEntity.IsSelected(false);
    }
}

public class ResetPathOnActionModeChanged : IReactiveSystem
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
            if (Pools.sharedInstance.core.hasPath)
                Pools.sharedInstance.core.pathEntity.IsDestroy(true);
        }
    }
}

public class ActionModeSystems : Feature
{
    ActionMode _mode;

    public ActionModeSystems(Pools pools, ActionMode mode) : base("ActionMode Systems")
    {
        _mode = mode;

        base.Add(pools.core.CreateSystem(new HighlightPathSystem()));
    }

    public override void Execute()
    {
        if (Pools.sharedInstance.parameters.actionMode.Mode == _mode)
            base.Execute();
    }
}