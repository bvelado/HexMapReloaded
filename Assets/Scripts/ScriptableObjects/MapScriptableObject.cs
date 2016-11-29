using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Map")]
public class MapScriptableObject : ScriptableObject {

    public string Title;
    public List<Tile> Tiles = new List<Tile>();

}
