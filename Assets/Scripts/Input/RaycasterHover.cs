using System;
using Entitas;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaycasterHover : MonoBehaviour, IActionModeChangedListener {

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
        enabled = (mode == ActionMode.Select || mode == ActionMode.Move);
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Layer.value))
        {
            if (hit.transform && hit.transform.GetComponent<IHoverable>() != null)
            {                
                if(Pools.sharedInstance.core.isHovered && 
                   Pools.sharedInstance.core.hoveredEntity != hit.transform.GetComponent<IHoverable>().GetEntity())
                {
                    Pools.sharedInstance.core.hoveredEntity.IsHovered(false);
                }
                hit.transform.GetComponent<IHoverable>().GetEntity().IsHovered(true);
                
            }
        }
        else if(!es.IsPointerOverGameObject())
        {
            if (Pools.sharedInstance.core.isHovered)
                Pools.sharedInstance.core.hoveredEntity.IsHovered(false);
        }
    }
}
