using UnityEngine;

public abstract class Shootable : MonoBehaviour
{
    public virtual void Awake()
    {
        gameObject.layer =
            9; // this sets the assigned gameobject the layer of 9, which is 'interactable'. This enables interaction script to run for that particular gameobject.
    }

    public abstract void OnDamage(float amount);
    public abstract void Death();
    public abstract void OnFocus();
    public abstract void OnFocusLost();
}