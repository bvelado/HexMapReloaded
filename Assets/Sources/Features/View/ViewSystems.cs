using System;
using System.Collections.Generic;
using Entitas;

public class HihghlightTileViewSystem : IReactiveSystem
{
    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.AllOf(ViewMatcher.TileView, ViewMatcher.Highlight).OnEntityAddedOrRemoved();
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach(var e in entities)
        {
            if (e.hasHighlight)
            {
                if (e.hasTileView)
                    e.tileView.View.Highlight(e.highlight.Mode);
            } else
            {
                if (e.hasTileView)
                    e.tileView.View.Highlight(HighlightMode.None);
            }
        }
    }
}
