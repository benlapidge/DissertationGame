using UnityEngine;

public class HealthPack : Interactable
{
    [SerializeField] private HealthSystem health;
    [SerializeField] public int healthIncrement = 20;

    private void Awake()
    {
        health = GameObject.Find("Player").GetComponent<HealthSystem>();
        gameObject.layer =
            9; // this sets the assigned gameobject the layer of 9, which is 'interactable'. This enables interaction script to run for that particular gameobject.
    }

    public override void OnFocus()
    {
        //Debug.Log("Looking at: " + gameObject.name);
    }

    public override void OnFocusLost()
    {
        //Debug.Log("Stopped looking at: " + gameObject.name);
    }

    public override void OnInteract()
    {
        health.IncreaseHealth(healthIncrement);
        Destroy(gameObject);
    }
}