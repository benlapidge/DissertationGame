using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public virtual void Awake()
    {
        gameObject.layer = 9; // this sets the assigned gameobject the layer of 9, which is 'interactable'. This enables interaction script to run for that particular gameobject.
    }

    public abstract void OnFocus();
    public abstract void OnFocusLost();
    public abstract void OnInteract();
}
