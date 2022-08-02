using UnityEngine;

public class TestDestroyableObject : Shootable
{
    [Header("Shootable Object Properties")] [SerializeField]
    private float health;

    private Animator animation;
    private ParticleSystem blood;

    private void Start()
    {
        animation = gameObject.GetComponent<Animator>();
        blood = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    public override void OnDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Death();
    }

    public override void Death()
    {
        blood.Play();
        animation.Play("DeathAnim");
        animation.StopPlayback();
    }

    public override void OnFocus()
    {
        Debug.Log("Looking at " + gameObject.name);
    }

    public override void OnFocusLost()
    {
        Debug.Log("Stopped looking at " + gameObject.name);
    }
}