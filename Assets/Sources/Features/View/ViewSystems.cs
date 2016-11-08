using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

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

            if (entity.hasMapPosition && entity.hasId)
            {
                var e = Pools.sharedInstance.view.CreateEntity();
                e.AddWorldPosition(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));

                GameObject tileGO = GameObject.Instantiate(Resources.Load("Prefabs/HexTile"), e.worldPosition.Position, Quaternion.identity, _tileContainer.transform) as GameObject;

                if(tileGO)
                {
                    tileGO.name = entity.tile.Description;
                    var tileView = tileGO.AddComponent<TileView>();

                    tileView.Initialize(entity.mapPosition.Position, entity.id.Id);

                    e.AddTileView(tileView)
                    .AddSelectedListener(tileView)
                    .AddId(entity.id.Id);
                } else
                {
                    throw new Exception("GameObject doesn't exist.");
                }
            } else
            {
                throw new Exception("Given entity has no mapPosition and/or ID.");
            }
        }
    }
}

/// <summary>
/// Update the HighlightComponent based on the addition or removal of the SelectedComponent on a Tile or Character
/// </summary>
public class HighlightSelectedTileSystem : IMultiReactiveSystem
{
    public TriggerOnEvent[] triggers
    {
        get
        {
            return new[]
            {
                Matcher.AllOf(CoreMatcher.Character, CoreMatcher.Selected).OnEntityAddedOrRemoved(),
                Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.Selected).OnEntityAddedOrRemoved()
            };
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            if (e.isSelected)
            {
                if (e.hasHighlight)
                    e.ReplaceHighlight(HighlightMode.Selected);
                else
                    e.AddHighlight(HighlightMode.Selected);
            }
            else
            {
                if (e.hasHighlight)
                    e.ReplaceHighlight(HighlightMode.None);
                else
                    e.AddHighlight(HighlightMode.None);
            }
        }
    }
}

/// <summary>
/// Update the HighlightComponent based on the addition or removal of the HoveredComponent on a Tile or Character
/// </summary>
public class HighlightHoveredSystem : IMultiReactiveSystem
{
    public TriggerOnEvent[] triggers
    {
        get
        {
            return new[]
            {
                Matcher.AllOf(CoreMatcher.Character, CoreMatcher.Hovered).OnEntityAddedOrRemoved(),
                Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.Hovered).OnEntityAddedOrRemoved()
            };
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            if (e.isHovered)
            {
                if (e.hasHighlight)
                {
                    if (e.highlight.Mode >= HighlightMode.Secondary)
                        e.ReplaceHighlight(HighlightMode.Secondary);
                }
                else
                    e.AddHighlight(HighlightMode.Secondary);
            } else
            {
                if (e.hasHighlight)
                {
                    if(e.highlight.Mode >= HighlightMode.Secondary)
                        e.ReplaceHighlight(HighlightMode.None);
                }                    
                else
                    e.AddHighlight(HighlightMode.None);
            }
        }
    }
}

/// <summary>
/// Updates the tile view to match the actual HighlightComponent value
/// </summary>
public class HighlightTileViewSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.Highlight).OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            Pools.sharedInstance.view.mapView.TileViewById.FindEntityAtIndex(e.id.Id).tileView.View.Highlight(e.highlight.Mode);
        }
    }
}