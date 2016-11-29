using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MapScriptableObject))]
public class MapEditor : Editor {
    
    private SerializedObject serializedMap;

    private SerializedProperty mapTitle;
    private SerializedProperty mapTiles;

    private Transform PreviewMapContainer;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Title"), GUIContent.none);
        EditorList.Show(serializedObject.FindProperty("Tiles"), EditorListOption.All);
        serializedObject.ApplyModifiedProperties();

        //if(serializedMap == null)
        //{
        //    serializedMap = new SerializedObject(target as MapScriptableObject);
        //    mapTitle = serializedMap.FindProperty("Title");
        //    mapTiles = serializedMap.FindProperty("Tiles");
        //}

        //serializedMap.Update();

        //DrawCustomInspector();

        //serializedMap.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (serializedMap == null)
        {
            serializedMap = new SerializedObject(target as MapScriptableObject);
            mapTitle = serializedMap.FindProperty("Title");
            mapTiles = serializedMap.FindProperty("Tiles");
        }

        if (PreviewMapContainer == null)
        {
            PreviewMapContainer = new GameObject("Map Preview").transform;
            PreviewMapContainer.transform.position = Vector3.zero;
        }   

       //foreach (var tile in (target as MapScriptableObject).Tiles)
       // {
       //     GameObject.Instantiate(tile.ViewPrefab, MapUtilities.MapToWorldPosition(tile.MapPosition), Quaternion.identity, PreviewMapContainer);
       // }
    }

    void DrawCustomInspector()
    {
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        GUILayout.Label("Map Editor  - " + target.name, EditorStyles.boldLabel);

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(mapTitle, new GUIContent("Name"));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        EditorList.Show(mapTiles, EditorListOption.All);

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
