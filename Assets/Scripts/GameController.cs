using UnityEngine;
using System.Collections;
using Entitas;

public class GameController : MonoBehaviour
{
    Systems _systems;

    void Start()
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        _systems = createSystems(pools);
        _systems.Initialize();
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    void OnDestroy()
    {
        _systems.TearDown();
    }

    Systems createSystems(Pools pools)
    {
        return new Feature("Systems")

        // Map
        .Add(pools.core.CreateSystem(new GenerateMapSystem()))
        .Add(pools.core.CreateSystem(new AddTileViewSystem()))
        .Add(pools.core.CreateSystem(new GenerateCharactersSystem()))
        .Add(pools.core.CreateSystem(new AddCharacterViewSystem()))

        // View
        .Add(pools.core.CreateSystem(new NotifySelectedListeners(pools.uI)))
        .Add(pools.core.CreateSystem(new NotifySelectedListeners(pools.view)));
    }
}