using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Interactable
{

    private Animator doorAnim;
    private bool doorOpen = false;
    [SerializeField] int doorNum = default;
    


    public override void Awake()
    {
        gameObject.layer = 9;
        doorAnim = gameObject.GetComponent<Animator>();
    }


    public override void OnFocus()
    {
        Debug.Log("Looking at " + gameObject.name);
        
    }

    public override void OnFocusLost()
    {
        Debug.Log("Stopped looking at " + gameObject.name);
    }

    public override void OnInteract()
    {
        if (!doorOpen)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;

        }
       
    }

    
}
