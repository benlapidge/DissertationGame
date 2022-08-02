using UnityEngine;

public class KeyScript : Interactable
{
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private int keyNum;

    public override void OnFocus()
    {
        //Debug.Log("Looking at key " + keyNum);
    }

    public override void OnFocusLost()
    {
        //Debug.Log("Stopped looking at key " + keyNum);
    }

    public override void OnInteract()
    {
        //Debug.Log("Interacted with key " + keyNum);
        inventory.SetKeys(keyNum);
        Destroy(gameObject);
    }
}