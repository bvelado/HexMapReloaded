using UnityEngine;
using System.Collections;

public static class TileUtilities {
    public const string TILE_GREEN = "Data/Tiles/TileGreen";
    public const string TILE_BROWN = "Data/Tiles/TileBrown";

    public static TileScriptableObject GetTileScriptableObject(string resourcePath)
    {
        return Resources.Load< TileScriptableObject>(resourcePath);
    }
}
