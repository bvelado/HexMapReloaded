using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class HihghlightTileViewSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.Highlight).OnEntityAddedOrRemoved();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            if (e.hasHighlight)
            {
                if (Pools.sharedInstance.view.mapView.TileViewById.FindEntityAtIndex(e.id.Id).hasTileView)
                    Pools.sharedInstance.view.mapView.TileViewById.FindEntityAtIndex(e.id.Id).tileView.View.Highlight(e.highlight.Mode);
            } else
            {
                if (Pools.sharedInstance.view.mapView.TileViewById.FindEntityAtIndex(e.id.Id).hasTileView)
                    Pools.sharedInstance.view.mapView.TileViewById.FindEntityAtIndex(e.id.Id).tileView.View.Highlight(HighlightMode.None);
            }
        }
    }
}

public class AddTileViewSystem : IReactiveSystem
{
    public static GameObject _tileContainer = new GameObject("Tile Container");

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            if (!Pools.sharedInstance.view.hasMapView)
                Pools.sharedInstance.view.CreateEntity().AddMapView(new IdIndex(Pools.sharedInstance.view, Matcher.AllOf(ViewMatcher.TileView, ViewMatcher.Id)));

            var e = Pools.sharedInstance.view.CreateEntity();
            e.AddWorldPosition(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));
            GameObject tileGO = GameObject.Instantiate(Resources.Load("Prefabs/HexTile"), e.worldPosition.Position, Quaternion.identity, _tileContainer.transform) as GameObject;
            tileGO.name = entity.tile.Description;
            var tileView = tileGO.AddComponent<TileView>();
            if (tileView)
            {
                tileView.Initialize(entity.mapPosition.Position);
                e.AddTileView(tileView);
                e.AddSelectedListener(tileView);
            }

            if (entity.hasId)
                e.AddId(entity.id.Id);
        }
    }
}