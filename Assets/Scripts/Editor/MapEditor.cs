using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MapEditor : EditorWindow {

    private string newMapName;

    private MapScriptableObject map;

    [MenuItem("Window/MapEditor")]
    static void Init()
    {
        MapEditor editor = (MapEditor)EditorWindow.GetWindow<MapEditor>();
        editor.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Map Editor", EditorStyles.boldLabel);
        if(!map)
        {
            GUILayout.BeginHorizontal();

            newMapName = GUILayout.TextField("New map", EditorStyles.textField);

            if (GUILayout.Button("Create", EditorStyles.miniButtonRight))
            {
                var newMap = ScriptableObject.CreateInstance<MapScriptableObject>();
                newMap.Tiles = new List<TileScriptableObject>();
                newMap.Title = newMapName;
                map = newMap;
            }

            if (GUILayout.Button("Load map", EditorStyles.miniButtonRight))
            {
                EditorGUIUtility.ShowObjectPicker<MapScriptableObject>(map, false, "", 0);
            }

            GUILayout.EndHorizontal();
        } else
        {

        }
        
    }

}
