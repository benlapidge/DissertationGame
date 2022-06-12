using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : Interactable
{

    [SerializeField] InventorySystem inventory = null;
    [SerializeField] int keyNum = default;
    public override void OnFocus()
    {
        Debug.Log("Looking at key " + keyNum);
    }

    public override void OnFocusLost()
    {
        Debug.Log("Stopped looking at key " + keyNum);
    }

    public override void OnInteract()
    {
        Debug.Log("Interacted with key " + keyNum);
        inventory.SetKeys(keyNum);
    }
   
}
