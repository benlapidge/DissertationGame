using UnityEngine;

public class DoorScript : Interactable
{
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private int doorNum;
    private Animator doorAnim;
    private bool doorOpen;
    [SerializeField] private AudioSource doorSounds = default;
    [SerializeField] private AudioClip doorunlocked = default;
    [SerializeField] private AudioClip doorlocked = default;


    public override void Awake()
    {
        gameObject.layer = 9;
        doorAnim = gameObject.GetComponent<Animator>();
        doorSounds = GetComponent<AudioSource>();
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
            doorSounds.PlayOneShot(doorunlocked);
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
        doorSounds.PlayOneShot(doorlocked);
    }
}