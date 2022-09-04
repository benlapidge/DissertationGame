using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;
    public static Action<float> OnRepair;
    public static Action<bool> OnDeath;

    [Header("Health System")] [SerializeField]
    private float maxHealth = 100;

    [SerializeField] private float timeToRegen = 3;
    [SerializeField] private float healthValueIncrement = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    [SerializeField] private bool regenActive;
    private float currentHealth;
    private bool dead;
    private Coroutine regeneratingHealth;
    private CharacterController player = default;
    private AdaptationEngine engine = default;


    private void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        engine = GameObject.Find("AdaptationEngine").GetComponent<AdaptationEngine>();
    }

    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }


    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        OnDamage?.Invoke(currentHealth);
        if (currentHealth <= 0)
            PlayerDeath();
        else if (regeneratingHealth != null)
            StopCoroutine(HealthRegen());

        regeneratingHealth = StartCoroutine(HealthRegen());
    }

    public void PlayerDeath(bool byTimeExpire = false)
    {
        if (byTimeExpire == false)
        {
            currentHealth = 0;
            if (regeneratingHealth != null)
                StopCoroutine(HealthRegen());
            Debug.Log("DEAD");
            dead = true;
            currentHealth = 50;
            OnDeath?.Invoke(dead);
            player.enabled = false;
            player.transform.position = engine.CurrentRoom().Find("RespawnPoint").transform.position;
            player.enabled = true;
            dead = false;
            OnDeath?.Invoke(dead);
            
        } else if (byTimeExpire == true)
        {
            
            
            if (regeneratingHealth != null)
                StopCoroutine(HealthRegen());
            Debug.Log("DEAD");
            dead = true;
            OnDeath?.Invoke(dead);
            player.enabled = false;
            player.transform.position = engine.CurrentRoom().Find("RespawnPoint").transform.position;
            player.enabled = true;
            dead = false;
            OnDeath?.Invoke(dead);
        }
        


    }

    private IEnumerator HealthRegen()
    {
        yield return new WaitForSeconds(timeToRegen);
        var timeToWait = new WaitForSeconds(healthTimeIncrement);
        if (regenActive)
            while (currentHealth < maxHealth)
            {
                currentHealth += healthValueIncrement;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                OnHeal?.Invoke(currentHealth);
                yield return timeToWait;
            }

        regeneratingHealth = null;
    }

    //for adaptation engine

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncreaseHealth(int healthAmount)
    {
        if (currentHealth < maxHealth)
        {
            if (currentHealth + healthAmount <= maxHealth)
            {
                currentHealth += healthAmount;

                OnRepair.Invoke(currentHealth);
            }
            else
            {
                currentHealth = maxHealth;
                OnRepair?.Invoke(currentHealth);
            }
        }
    }
}