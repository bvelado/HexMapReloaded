using System;
using Entitas;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class RaycasterMove : MonoBehaviour, IActionModeChangedListener {

    public EventSystem es;
    public LayerMask Layer;

    private RaycastHit hit;

    private Group mapEntities;

    private Coroutine pathRequest;
    private Tween moveSequence;

    void Start()
    {
        Pools.sharedInstance.uI.CreateEntity().AddActionModeChangedListener(this);
        enabled = false;

        if (mapEntities == null)
            mapEntities = Pools.sharedInstance.core.GetGroup(Matcher.AllOf(CoreMatcher.Tile, CoreMatcher.MapPosition));
    }

    public void ActionModeChanged(ActionMode mode)
    {
        enabled = (mode == ActionMode.Move);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Layer.value))
            {
                if (hit.transform && hit.transform.GetComponent<IWalkable>() != null && hit.collider.GetComponent<IWalkable>().IsWalkable())
                {
                    //if (!Pools.sharedInstance.core.hasPath)
                    //    Pools.sharedInstance.core.CreateEntity().AddPath(new Vector3[0]);

                    //if (TryAddTileToPath(hit.transform.GetComponent<IWalkable>().GetMapPosition()))
                    //{
                    //    Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(hit.transform.GetComponent<IWalkable>().GetMapPosition()).ReplaceHighlight(HighlightMode.Primary);
                    //}

                    if (pathRequest != null)
                    {
                        StopCoroutine(pathRequest);
                    }

                    pathRequest = StartCoroutine(PathUtilities.ProcessPathfinding(mapEntities.GetEntities(),
                        Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(Pools.sharedInstance.core.controllableEntity.mapPosition.Position),
                        Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(hit.transform.GetComponent<IWalkable>().GetMapPosition()),
                        DebugPath));
                }
            }
            else if(!es.IsPointerOverGameObject())
            {
                if (Pools.sharedInstance.core.isSelected)
                    Pools.sharedInstance.core.selectedEntity.IsSelected(false);
            }
        }
    }

    public void DebugPath(Entity[] path)
    {
        Pools.sharedInstance.parameters.ReplaceActionMode(ActionMode.None);

        if(moveSequence != null && moveSequence.IsPlaying())
            moveSequence.Kill(false);

        var controllableCharacterView = Pools.sharedInstance.view.charactersView.CharacterViewById.FindEntityAtIndex(Pools.sharedInstance.core.controllableEntity.id.Id);
        var controllableCharacter = Pools.sharedInstance.core.controllableEntity;

        List<Vector3> worldPositions = new List<Vector3>();
        foreach(var entity in path)
        {
            worldPositions.Add(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));
        }
        
        moveSequence = controllableCharacterView.characterView.View.transform.DOLocalPath(worldPositions.ToArray(), path.Length * 0.6f, PathType.Linear)
            .SetEase(Ease.Linear)
            // Update la position à chaque waypoint
            .OnWaypointChange((i) => { if (i != 0) controllableCharacter.ReplaceMapPosition(path[i - 1].mapPosition.Position); })
            .OnComplete(() => Pools.sharedInstance.parameters.ReplaceActionMode(ActionMode.Select));
    }

    //bool TryAddTileToPath(Vector3 MapPosition)
    //{
    //    Vector3[] PathCopy = Pools.sharedInstance.core.path.MapPositions;

    //    // Check if the last path tile exist and is neighbor
    //    if(PathCopy.Length > 0)
    //    {
    //        Vector3 PathLastMapPosition = PathCopy[PathCopy.Length - 1];
    //        if(!MapUtilities.IsNeighbor(MapPosition, PathLastMapPosition)) {
    //            return false;
    //        }
    //    }

    //    PathCopy = new Vector3[Pools.sharedInstance.core.path.MapPositions.Length +1];
    //    for(int i = 0; i < Pools.sharedInstance.core.path.MapPositions.Length; i++)
    //    {
    //        PathCopy[i] = Pools.sharedInstance.core.path.MapPositions[i];
    //    }
    //    PathCopy[Pools.sharedInstance.core.path.MapPositions.Length] = MapPosition;

    //    Pools.sharedInstance.core.ReplacePath(PathCopy);

    //    return true;
    //}
}
