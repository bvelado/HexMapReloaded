using UnityEngine;
using System.Collections;

public class TileRaycaster : MonoBehaviour {

    public LayerMask TileLayer;

    private RaycastHit hit;

	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, TileLayer.value))
            {
                if (hit.transform)
                {
                    hit.collider.GetComponent<ISelectedHandler>().OnSelected();
                }
            }
        }
        
	}
}
