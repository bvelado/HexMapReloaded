using UnityEngine;
using System.Collections;
using Entitas;

public class GameController : MonoBehaviour
{
    Systems _systems;
    Systems _actionModeMoveSystems;
    void Start()
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        _systems = createSystems(pools);
        _systems.Initialize();

        pools.parameters.CreateEntity().AddActionMode(ActionMode.Select);

        _actionModeMoveSystems = new ActionModeSystems(pools, ActionMode.Move);
        _actionModeMoveSystems.Initialize();
        
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
        _actionModeMoveSystems.Execute();
        _actionModeMoveSystems.Cleanup();
    }

    void OnDestroy()
    {
        _systems.TearDown();
        _actionModeMoveSystems.TearDown();
    }

    Systems createSystems(Pools pools)
    {
        return new Feature("Systems")

        .Add(pools.input.CreateSystem(new HandleInputSystem()))

        // Map
        .Add(pools.core.CreateSystem(new GenerateMapSystem()))
        .Add(pools.core.CreateSystem(new AddTileViewSystem()))
        .Add(pools.core.CreateSystem(new GenerateCharactersSystem()))
        .Add(pools.core.CreateSystem(new AddCharacterViewSystem()))

        // Logic
        .Add(pools.parameters.CreateSystem(new ResetSelectedOnActionModeChanged()))
        .Add(pools.parameters.CreateSystem(new ResetPathOnActionModeChanged()))

        // Core listeners
        .Add(pools.core.CreateSystem(new NotifyControlledListenersSystem(pools.core)))

        .Add(pools.parameters.CreateSystem(new NotifyActionModeChangedListenersSystem(pools.core)))

        // UI & View listeners
        .Add(pools.core.CreateSystem(new NotifySelectedListenersSystem(pools.uI)))
        .Add(pools.core.CreateSystem(new NotifySelectedListenersSystem(pools.view)))

        .Add(pools.core.CreateSystem(new NotifyControlledListenersSystem(pools.uI)))
        .Add(pools.core.CreateSystem(new NotifyControlledListenersSystem(pools.view)))

        .Add(pools.parameters.CreateSystem(new NotifyActionModeChangedListenersSystem(pools.uI)))
        .Add(pools.parameters.CreateSystem(new NotifyActionModeChangedListenersSystem(pools.view)))

        .Add(pools.core.CreateSystem(new HighlightHoveredSystem()))
        .Add(pools.core.CreateSystem(new HighlightSelectedTileSystem()))
        
        .Add(pools.core.CreateSystem(new HighlightTileViewSystem()))

        .Add(pools.input.CreateSystem(new EndTurnSystem()))

        .Add(pools.core.CreateSystem(new DestroySystem()))
        .Add(pools.view.CreateSystem(new DestroySystem()))
        .Add(pools.uI.CreateSystem(new DestroySystem()));
    }
}