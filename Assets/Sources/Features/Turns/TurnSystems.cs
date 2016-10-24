using System;
using System.Collections.Generic;
using Entitas;

public class EndTurnSystem : IReactiveSystem, ISetPool
{
    Group charactersWithTurnOrder;

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(InputMatcher.Input).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            if (Pools.sharedInstance.core.isControllable)
            {
                bool turnHasEnded = false;

                Entity lowestTurnOrderCharacterEntity;
                if (charactersWithTurnOrder.count > 0)
                    lowestTurnOrderCharacterEntity = charactersWithTurnOrder.GetEntities()[0];
                else
                    continue;
                
                foreach(var character in charactersWithTurnOrder.GetEntities())
                {
                    if (turnHasEnded)
                        continue;

                    if (character.turnOrder.OrderIndex < lowestTurnOrderCharacterEntity.turnOrder.OrderIndex)
                        lowestTurnOrderCharacterEntity = character;

                    if(character.turnOrder.OrderIndex == Pools.sharedInstance.core.controllableEntity.turnOrder.OrderIndex + 1)
                    {
                        Pools.sharedInstance.core.controllableEntity.IsControllable(false);
                        character.IsControllable(true);
                        turnHasEnded = true;
                    }
                }

                if(!turnHasEnded)
                {
                    Pools.sharedInstance.core.controllableEntity.IsControllable(false);
                    lowestTurnOrderCharacterEntity.IsControllable(true);
                }
            }

            e.IsDestroy(true);
        }
    }

    public void SetPool(Pool pool)
    {
        charactersWithTurnOrder = Pools.sharedInstance.core.GetGroup(Matcher.AllOf(CoreMatcher.Character, CoreMatcher.TurnOrder));
    }
}