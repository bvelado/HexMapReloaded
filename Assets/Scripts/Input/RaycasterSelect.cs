using Entitas;
using UnityEngine;

public class RaycasterSelect : MonoBehaviour {

    public LayerMask Layer;

    private RaycastHit hit;

	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, Layer.value))
            {
                if (hit.transform)
                {
                    hit.collider.GetComponent<ISelectable>().Select();
                } else
                {
                    Pools.sharedInstance.core.selectedEntity.IsSelected(false);
                }
            }
        }
        
	}
}
