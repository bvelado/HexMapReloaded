using UnityEngine;

[System.Serializable]
public class Tile {

    public Vector3 MapPosition;
    public TileScriptableObject TileSO;
    public int Id;

    public Tile(Vector3 mapPosition, int iD, TileScriptableObject tileSO = null)
    {
        MapPosition = mapPosition;
        Id = iD;
        if(tileSO == null)
        {
            TileSO = Resources.Load<TileScriptableObject>("Data/TileDefault");
        } else
        {
            TileSO = tileSO;
        }
    }

}

[CreateAssetMenu(menuName ="Tile")]
[System.Serializable]
public class TileScriptableObject : ScriptableObject
{
    public GameObject ViewPrefab;
}