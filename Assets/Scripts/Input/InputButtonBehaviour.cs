using UnityEngine;
using System.Collections;
using Entitas;

public class InputButtonBehaviour : MonoBehaviour {

    public InputIntent Intent;

    public void ButtonPressed()
    {
        Pools.sharedInstance.input.CreateEntity().AddInput(Intent);
    }

}
