using UnityEngine;

public interface IWalkable
{
    bool IsWalkable();
    Vector3 GetMapPosition();
}