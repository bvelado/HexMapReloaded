using UnityEngine;
using Entitas;
using System;

public class TileView : MonoBehaviour, ISelectable, ISelectedListener {

    private Vector3 _mapPosition;
    public Vector3 WorldPosition
    {
        get
        {
            return MapUtilities.MapToWorldPosition(_mapPosition);
        }
    }
    
    public void Initialize(Vector3 MapPosition)
    {
        _mapPosition = MapPosition;
    }

    public Entity GetEntity()
    {
        return Pools.sharedInstance.core.map.Map.FindEntityAtMapPosition(_mapPosition);
    }

    public void Select()
    {
        GetEntity().IsSelected(true);
    }

    public void SelectedChanged(Entity selectedEntity)
    {
        if(selectedEntity != GetEntity())
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        } else
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }
}
