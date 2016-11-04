using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class GenerateMapSystem : IInitializeSystem
{
    public void Initialize()
    {
        if (!Pools.sharedInstance.core.hasMap)
            Pools.sharedInstance.core.CreateEntity()
                .AddMap(
                    new PositionIndex(Pools.sharedInstance.core, Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition)), 
                    new IdIndex(Pools.sharedInstance.core, Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.Id)));

        int width = 5;
        int height = 5;
        int radius = Mathf.Max(width, height);

        int i = 0;
        for (int x = radius; x >= -radius; x--)
        {
            for (int y = radius; y >= -radius; y--)
            {
                if ((-x - y) >= -radius && (-x - y) <= radius)
                {
                    Pools.sharedInstance.core.CreateEntity()
                        .AddTile("Tile @ ( " + x + " , " + y + " , " + (-x - y) + " )")
                        .AddMapPosition(new Vector3(x, y, -x - y))
                        .AddId(i);
                    i++;
                }
            }
        }
    }
}

public class HighlightPathSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return CoreMatcher.Path.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            foreach(var mapPosition in e.path.MapPositions)
            {
                Debug.Log(Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(mapPosition).hasTileView);
            }
        }
    }
}