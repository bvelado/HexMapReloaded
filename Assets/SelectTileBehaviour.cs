using UnityEngine;
using System.Collections;
using System;

public interface ISelectedHandler
{
    void OnSelected();
}

public class SelectTileBehaviour : MonoBehaviour, ISelectedHandler
{
    public void OnSelected()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
