using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : Interactable
{

    
    public override void OnFocus()
    {
        
        Debug.Log("Looking at " + gameObject.name);
        GetComponentInChildren<Light>().color = Color.blue;
    }

    public override void OnFocusLost()
    {
        Debug.Log("Stopped looking at " + gameObject.name);
        GetComponentInChildren<Light>().color = Color.white;

    }

    public override void OnInteract()
    {
        
        Debug.Log("Interacted with " + gameObject.name);
        if (GetComponentInChildren<Light>().enabled == false)
        {
            GetComponentInChildren<Light>().enabled = true;
        } else
        {
            GetComponentInChildren<Light>().enabled = false;
        }
        

    }
}
