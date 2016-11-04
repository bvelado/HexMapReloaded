using System;
using System.Collections.Generic;
using Entitas;

public class HandleInputSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return InputMatcher.Input.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            if(e.input.Intent == InputIntent.BeginMove)
            {
                Pools.sharedInstance.parameters.actionModeEntity.ReplaceActionMode(ActionMode.Move);

                e.IsDestroy(true);
            }
        }
    }
}
