using UnityEngine;

public class DoorScript : Interactable
{
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private int doorNum;
    private Animator doorAnim;
    private bool doorOpen;


    public override void Awake()
    {
        gameObject.layer = 9;
        doorAnim = gameObject.GetComponent<Animator>();
    }


    public override void OnFocus()
    {
        //Debug.Log("Looking at " + gameObject.name);
    }

    public override void OnFocusLost()
    {
       // Debug.Log("Stopped looking at " + gameObject.name);
    }

    public override void OnInteract()
    {
        if (!doorOpen && inventory.GetKeys().Contains(doorNum))
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
    }
}