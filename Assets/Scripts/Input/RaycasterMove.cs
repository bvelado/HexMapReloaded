using System;
using Entitas;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaycasterMove : MonoBehaviour, IActionModeChangedListener {

    public EventSystem es;
    public LayerMask Layer;

    private RaycastHit hit;

    void Start()
    {
        Pools.sharedInstance.uI.CreateEntity().AddActionModeChangedListener(this);
        enabled = false;
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
                    if (!Pools.sharedInstance.core.hasPath)
                        Pools.sharedInstance.core.CreateEntity().AddPath(new Vector3[0]);

                    TryAddTileToPath(hit.transform.GetComponent<IWalkable>().GetMapPosition());
                }
            }
            else if(!es.IsPointerOverGameObject())
            {
                if (Pools.sharedInstance.core.isSelected)
                    Pools.sharedInstance.core.selectedEntity.IsSelected(false);
            }
        }
    }

    bool TryAddTileToPath(Vector3 MapPosition)
    {
        Vector3[] PathCopy = Pools.sharedInstance.core.path.MapPositions;

        // Check if the last path tile exist and is neighbor
        if(PathCopy.Length > 0)
        {
            Vector3 PathLastMapPosition = PathCopy[PathCopy.Length - 1];
            if(!MapUtilities.IsNeighbor(MapPosition, PathLastMapPosition)) {
                return false;
            }
        }

        PathCopy = new Vector3[Pools.sharedInstance.core.path.MapPositions.Length +1];
        for(int i = 0; i < Pools.sharedInstance.core.path.MapPositions.Length; i++)
        {
            PathCopy[i] = Pools.sharedInstance.core.path.MapPositions[i];
        }
        PathCopy[Pools.sharedInstance.core.path.MapPositions.Length] = MapPosition;

        Pools.sharedInstance.core.ReplacePath(PathCopy);

        return true;
    }
}
