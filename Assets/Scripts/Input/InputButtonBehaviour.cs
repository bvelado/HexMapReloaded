using UnityEngine;
using System.Collections;
using Entitas;

public class InputButtonBehaviour : MonoBehaviour {

    public InputIntent Intent;

    public void ButtonPressed()
    {
        print("Button pressed : " + name + " with intent : " + Intent.ToString() + ".");
        Pools.sharedInstance.input.CreateEntity().AddInput(Intent);
    }

}
