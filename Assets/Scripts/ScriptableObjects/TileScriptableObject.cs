using UnityEngine;

[System.Serializable]
public class Tile {

    public Vector3 MapPosition;
    public TileScriptableObject TileSO;

}

[CreateAssetMenu(menuName ="Tile")]
[System.Serializable]
public class TileScriptableObject : ScriptableObject
{
    public Transform ViewPrefab;
}