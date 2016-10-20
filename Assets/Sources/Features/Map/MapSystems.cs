using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class GenerateMapSystem : IInitializeSystem
{
    public void Initialize()
    {
        if (!Pools.sharedInstance.core.hasMap)
            Pools.sharedInstance.core.CreateEntity().AddMap(new PositionIndex(Pools.sharedInstance.core, Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition)));

        int width = 5;
        int height = 5;
        int radius = Mathf.Max(width, height);

        for (int x = radius; x >= -radius; x--)
        {
            for (int y = radius; y >= -radius; y--)
            {
                if ((-x - y) >= -radius && (-x - y) <= radius)
                {
                    Pools.sharedInstance.core.CreateEntity()
                        .AddTile("Tile @ ( " + x + " , " + y + " , " + (-x - y) + " )")
                        .AddMapPosition(new Vector3(x, y, -x - y));
                }
            }
        }
    }
}

public class AddTileViewSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var entity in entities)
        {
            var e = Pools.sharedInstance.view.CreateEntity();
            e.AddWorldPosition(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));
            GameObject tileGO = GameObject.Instantiate(Resources.Load("Prefabs/HexTile"), e.worldPosition.Position, Quaternion.identity) as GameObject;
            tileGO.name = entity.tile.Description;
            var tileView = tileGO.AddComponent<TileView>();
            if (tileView)
            {
                tileView.Initialize(entity.mapPosition.Position);
                e.AddTileView(tileView);
                e.AddSelectedListener(tileView);
            }   
        }
    }
}
