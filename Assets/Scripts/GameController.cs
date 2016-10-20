using UnityEngine;
using System.Collections;
using Entitas;

public class GameController : MonoBehaviour
{
    Systems _systems;

    void Start()
    {
        Random.seed = 42;

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

        //// Input
        .Add(pools.core.CreateSystem(new GenerateMapSystem()))
        .Add(pools.core.CreateSystem(new AddTileViewSystem()));
    }
}