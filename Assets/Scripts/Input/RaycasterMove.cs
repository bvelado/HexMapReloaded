using System;
using Entitas;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class RaycasterMove : MonoBehaviour, IActionModeChangedListener
{

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
                    if (pathRequest != null)
                    {
                        if (moveSequence != null && moveSequence.IsPlaying())
                            moveSequence.Kill(false);
                        StopCoroutine(pathRequest);
                    }

                    if (MapUtilities.GetDistance(
                        Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(Pools.sharedInstance.core.controllableEntity.mapPosition.Position),
                        Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(hit.transform.GetComponent<IWalkable>().GetMapPosition())) <=
                        Pools.sharedInstance.core.controllableEntity.character.Unit.Stats.MovementPoints.GetFinalValue())
                    {
                        pathRequest = StartCoroutine(PathUtilities.ProcessPathfinding(mapEntities.GetEntities(),
                    Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(Pools.sharedInstance.core.controllableEntity.mapPosition.Position),
                    Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(hit.transform.GetComponent<IWalkable>().GetMapPosition()),
                    DebugPath));
                    }
                    else
                    {
                        print("Not enough MovP.");
                        Stack<int> test = new Stack<int>();
                        
                    }
                }
            }
            else if (!es.IsPointerOverGameObject())
            {
                if (Pools.sharedInstance.core.isSelected)
                    Pools.sharedInstance.core.selectedEntity.IsSelected(false);
            }
        }
    }

    public void DebugPath(Entity[] path)
    {
        Pools.sharedInstance.parameters.ReplaceActionMode(ActionMode.None);

        var controllableCharacterView = Pools.sharedInstance.view.charactersView.CharacterViewById.FindEntityAtIndex(Pools.sharedInstance.core.controllableEntity.id.Id);
        var controllableCharacter = Pools.sharedInstance.core.controllableEntity;

        List<Vector3> worldPositions = new List<Vector3>();
        foreach (var entity in path)
        {
            worldPositions.Add(MapUtilities.MapToWorldPosition(entity.mapPosition.Position));
        }

        moveSequence = controllableCharacterView.characterView.View.transform.DOLocalPath(worldPositions.ToArray(), path.Length * 0.6f, PathType.Linear)
            .SetEase(Ease.Linear)
            // Update la position à chaque waypoint
            .OnWaypointChange(
            (i) =>
            {
                if (i != 0)
                {
                    controllableCharacter.ReplaceMapPosition(path[i - 1].mapPosition.Position);
                    controllableCharacter.character.Unit.Stats.MovementPoints.AddFinalModifier(new FinalStatModifier(-1, 0, 1, controllableCharacter.character.Unit.Stats.MovementPoints));
                    print(controllableCharacter.character.Unit.Stats.MovementPoints.GetFinalValue());
                }
            })
            .OnComplete(() => Pools.sharedInstance.parameters.ReplaceActionMode(ActionMode.Select));
    }
}