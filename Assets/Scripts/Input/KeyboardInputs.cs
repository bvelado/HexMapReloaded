using Entitas;
using UnityEngine;

public class KeyboardInputs : MonoBehaviour {
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pools.sharedInstance.parameters.actionModeEntity.ReplaceActionMode(ActionMode.Select);
        }    
    }
}
