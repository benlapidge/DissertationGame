using UnityEngine;

public class KeyScript : Interactable
{
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private int keyNum;
    [SerializeField] private AudioSource audio = default;
    [SerializeField] private AudioClip pickup = default;
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
        audio.PlayOneShot(pickup,1);
        inventory.SetKeys(keyNum);
        Destroy(gameObject,0.5f);
    }
    
    
}