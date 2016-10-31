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
                if (hit.transform && hit.transform.GetComponent<ISelectable>() != null)
                {
                    hit.collider.GetComponent<ISelectable>().Select();
                }
            }
            else if(!es.IsPointerOverGameObject())
            {
                if (Pools.sharedInstance.core.isSelected)
                    Pools.sharedInstance.core.selectedEntity.IsSelected(false);
            }
        }
    }
}
