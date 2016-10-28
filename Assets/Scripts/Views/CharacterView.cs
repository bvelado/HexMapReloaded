using UnityEngine;
using System.Collections;
using Entitas;
using System;

public class CharacterView : MonoBehaviour, ISelectable, ISelectedListener, IControlledListener
{
    private Vector3 _mapPosition;
    public Vector3 WorldPosition
    {
        get
        {
            return MapUtilities.MapToWorldPosition(_mapPosition);
        }
    }
    private int _id;
    public int ID
    {
        get { return _id; }
    }

    private Color _baseColor;

    public void Initialize(Vector3 MapPosition, int id)
    {
        _mapPosition = MapPosition;
        _baseColor = GetComponent<MeshRenderer>().material.color;
        _id = id;
    }

    public void Select()
    {
        if (Pools.sharedInstance.core.isSelected)
            Pools.sharedInstance.core.selectedEntity.IsSelected(false);
        GetEntity().IsSelected(true);
    }

    public Entity GetEntity()
    {
        return Pools.sharedInstance.core.characters.CharactersByID.FindEntityAtIndex(ID);
    }

    public void SelectedChanged(Entity selectedEntity)
    {
        UpdateView();
    }

    public void ControlledChanged(Entity controlledEntity)
    {
        UpdateView();
    }

    public void UpdateView()
    {
        if (GetEntity().isControllable)
            GetComponent<MeshRenderer>().material.color = Color.blue;
        else if(!GetEntity().isControllable && GetEntity().isSelected)
            GetComponent<MeshRenderer>().material.color = Color.green;
        else
            GetComponent<MeshRenderer>().material.color = _baseColor;
    }
}
