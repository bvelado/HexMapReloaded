using UnityEngine;
using System.Collections;
using Entitas;

public class MapEditorController : MonoBehaviour {
    void Start()
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        SetupMap();
    }

    void SetupMap()
    {
        if (!Pools.sharedInstance.editor.hasMap)
            Pools.sharedInstance.editor.CreateEntity()
                .AddMap(
                    new PositionIndex(Pools.sharedInstance.core, Matcher.AllOf(EditorMatcher.Tile, EditorMatcher.MapPosition)),
                    new IdIndex(Pools.sharedInstance.core, Matcher.AllOf(EditorMatcher.Tile, EditorMatcher.Id)));
    }
}