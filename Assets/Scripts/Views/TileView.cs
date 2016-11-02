using UnityEngine;
using Entitas;
using System;

public class TileView : MonoBehaviour, ISelectable, IWalkable, IHighlightable, ISelectedListener {

    private int id;
    private Vector3 _mapPosition;
    public Vector3 WorldPosition
    {
        get
        {
            return MapUtilities.MapToWorldPosition(_mapPosition);
        }
    }

    private Color _baseColor;

    public void Initialize(Vector3 MapPosition)
    {
        _mapPosition = MapPosition;
        _baseColor = GetComponent<MeshRenderer>().material.color;
    }

    public Entity GetEntity()
    {
        return Pools.sharedInstance.core.map.TilesByMapPosition.FindEntityAtMapPosition(_mapPosition);
    }

    public void Select()
    {
        if(Pools.sharedInstance.core.isSelected)
            Pools.sharedInstance.core.selectedEntity.IsSelected(false);
        GetEntity().IsSelected(true);
    }

    public void SelectedChanged(Entity selectedEntity)
    {
        if(selectedEntity == null || selectedEntity != GetEntity())
        {
            GetComponent<MeshRenderer>().material.color = _baseColor;
        } else
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }

    public bool IsWalkable()
    {
        // TODO : CHANGE IT 
        return true;
    }

    public Vector3 GetMapPosition()
    {
        return _mapPosition;
    }

    public void Highlight(HighlightMode Mode)
    {
        switch (Mode)
        {
            case HighlightMode.Selected:
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case HighlightMode.Primary:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case HighlightMode.Secondary:

            case HighlightMode.None:
            default:

                GetComponent<MeshRenderer>().material.color = _baseColor;

                break;
        }
    }
}
