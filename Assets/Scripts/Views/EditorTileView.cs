using UnityEngine;
using Entitas;
using System;

public class EditorTileView : MonoBehaviour {

    private int _id;
    private Vector3 _mapPosition;
    public Vector3 MapPosition
    {
        get
        {
            return _mapPosition;
        }
    }
    public Vector3 WorldPosition
    {
        get
        {
            return MapUtilities.MapToWorldPosition(_mapPosition);
        }
    }

    public void Initialize(Vector3 mapPosition)
    {
        _mapPosition = mapPosition;
    }
}
