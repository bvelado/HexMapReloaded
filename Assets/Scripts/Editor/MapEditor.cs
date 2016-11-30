using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Entitas;

public class MapEditor : EditorWindow {
    
    static private MapScriptableObject map;
    static private SerializedObject serializedMap;

    static private SerializedProperty mapTitle;
    static private SerializedProperty mapTiles;

    static private Dictionary<Vector3, EditorTileView> tilesDrawn = new Dictionary<Vector3, EditorTileView>();

    static private Transform ViewContainer;

    private RaycastHit hit;
    private EditorTileView currentTileView;

    private Vector3[] neighborsPositions;
    
    [MenuItem("Window/Map Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapEditor window = (MapEditor)EditorWindow.GetWindow(typeof(MapEditor));
        window.Show();
        window.autoRepaintOnSceneChange = true;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    void OnSelectionChange()
    {
        HandleSelectionChanged();
    }

    void OnGUI()
    {
        if(serializedMap != null)
        {
            serializedMap.Update();
            EditorGUILayout.PropertyField(serializedMap.FindProperty("Title"), GUIContent.none);
            EditorList.Show(serializedMap.FindProperty("Tiles"), EditorListOption.ListLabel | EditorListOption.Buttons);
            
            if (GUILayout.Button("Draw map", GUILayout.ExpandWidth(true)))
            {
                DrawAllTiles();
            }

            if(tilesDrawn.Count > 0)
            {
                if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true)))
                {
                    Clear();
                }
            }

            serializedMap.ApplyModifiedProperties();
        }  else
        {
            if(Selection.activeObject != null && Selection.activeObject.GetType() == typeof(MapScriptableObject))
            {
                if (GUILayout.Button("Load"))
                {
                    map = Selection.activeObject as MapScriptableObject;
                    serializedMap = new SerializedObject(Selection.activeObject as MapScriptableObject);
                }
            } else
            {
                EditorGUILayout.HelpBox("No map selected", MessageType.Warning);
            }
        }
    }
    
    void DrawAllTiles()
    {
        if (tilesDrawn == null)
            tilesDrawn = new Dictionary<Vector3, EditorTileView>();

        Clear();

        if (ViewContainer == null)
        {
            ViewContainer = new GameObject(map.Title).transform;
        } else
        {
            ViewContainer.gameObject.name = map.Title;
        }

        var tiles = map.Tiles;
        foreach (var tile in tiles)
        {
            GameObject tileView = (GameObject)Instantiate(tile.TileSO.ViewPrefab, MapUtilities.MapToWorldPosition(tile.MapPosition), Quaternion.identity, ViewContainer);
            tileView.name = tile.MapPosition.ToString();
            if (tileView.GetComponent<TileView>() != null)
                Destroy(tileView.GetComponent<TileView>());
            tileView.AddComponent<EditorTileView>();
            tileView.GetComponent<EditorTileView>().Initialize(tile.MapPosition);
            tilesDrawn.Add(tile.MapPosition, tileView.GetComponent<EditorTileView>());
        }
    }

    void DrawTileHandles()
    {
        if(neighborsPositions == null)
        {
            List<Vector3> neighbors = new List<Vector3>();
            foreach(var direction in MapUtilities.GetAllDirections())
            {
                if(!tilesDrawn.ContainsKey(currentTileView.MapPosition + direction))
                {
                    neighbors.Add(MapUtilities.MapToWorldPosition(currentTileView.MapPosition + direction));
                }
            }
            neighborsPositions = neighbors.ToArray();
        } else
        {
            
            foreach(var neighbor in neighborsPositions)
            {
                if (Handles.Button(neighbor, Quaternion.identity, 0.5f, .5f, Handles.SphereCap)) {
                    map.Tiles.Add(new Tile(MapUtilities.WorldToMapPosition(neighbor), -1, TileUtilities.GetTileScriptableObject(TileUtilities.TILE_GREEN)));
                    Repaint();
                    DrawAllTiles();
                }
            }
        }
        
    }

    void HandleSelectionChanged()
    {
        if(Selection.activeObject != null && Selection.activeObject.GetType() == typeof(MapScriptableObject) && (Selection.activeObject as MapScriptableObject) != map)
        {
            Clear();
            map = Selection.activeObject as MapScriptableObject;
            serializedMap = new SerializedObject(Selection.activeObject as MapScriptableObject);
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<EditorTileView>() != null)
        {
            currentTileView = Selection.activeGameObject.GetComponent<EditorTileView>();
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        } else
        {
            currentTileView = null;
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        neighborsPositions = null;

        Repaint();
    }

    void Clear()
    {
        foreach(var tile in tilesDrawn)
        {
            if(tile.Value.gameObject != null) DestroyImmediate(tile.Value.gameObject);
        }
        
        tilesDrawn.Clear();

        if(ViewContainer != null && ViewContainer.gameObject != null)
        {
            DestroyImmediate(ViewContainer.gameObject);
            ViewContainer = null;
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (currentTileView != null)
        {
            Handles.color = Color.red;
            DrawTileHandles();
        }
    }
}
