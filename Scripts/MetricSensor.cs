using System;
using UnityEngine;

public class MetricSensor : MonoBehaviour
{
    [Header("Parameters for this tracker")] [SerializeField]
    private HealthSystem health;

    [SerializeField] private int roomNumber;
    private float enterHealth;
    private float exitHealth;
    private bool playerEntered;
    private bool playerExited;
    public bool startTimer;
    public float currentTime;
    [SerializeField] private float timeLimit;
    private bool playerDiedInChamber;
    private bool playerDiedFromTime;
    //for UI
    public static Action<float> OnTimerStart;
    private void Awake()
    {
        currentTime = timeLimit;
        health = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
        HealthSystem.OnDeath += RestartTimerOnDeath;
        HealthSystem.OnDeath += PlayerDiedInChamber;
    }
    
    private void Update()
    {
        Countdown(startTimer);
    }
 private void PlayerDiedInChamber(bool dead)
 {
     if (playerEntered && dead)
     {
         playerDiedInChamber = true;
     }
 }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerEntered)
        {
            enterHealth = health.GetCurrentHealth();
            StartTimer(true);
            Debug.Log("Player entered trigger zone with health of " + enterHealth);
            playerEntered = true;
        }
        else if (other.CompareTag("Player") && playerEntered)
        {
            StartTimer(true);
            Debug.Log("Player reentered trigger zone with health of " + enterHealth);
        }
        
        
    }

    private void RestartTimerOnDeath(bool dead)
    {
        if (dead)
            currentTime = timeLimit;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exitHealth = health.GetCurrentHealth();
            StartTimer(false);
            Debug.Log("Player left trigger zone with health of " + exitHealth);
            Debug.Log("Player took "+ReturnTimeTaken()+"to complete room");
        }

        if (enterHealth != exitHealth) Debug.Log("Player lost health of: " + (enterHealth - exitHealth));
        playerExited = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (health.GetCurrentHealth() > enterHealth)
            {
                enterHealth = health.GetCurrentHealth();
                Debug.Log("NEW ENTER HEALTH IS: " + enterHealth);
                //if player has a healthpack, their enter health will update to reflect this, and therefore any further loss will be correctly counted.
            }
        }
        
        //account for if player consumes health pack

        
    }

    //include method to return all values from this 'sensor' to adaptation engine

    public float EnterHealth()
    {
        
        return enterHealth;
    }

    public float ExitHealth()
    {
        if (playerDiedInChamber)
        {
            return enterHealth - (enterHealth-1);
        }
        
            return exitHealth;

    }

    public bool PlayerPresent()
    {
        return playerEntered;
    }

    public bool PlayerExited()
    {
        return playerExited;
    }
    
    
    //timer
    
    public void StartTimer(bool activate)
    {
        startTimer = activate;
    }

    private void Countdown(bool start)
    {
        if (start){
        currentTime -= Time.deltaTime;
        OnTimerStart?.Invoke(currentTime);
        if (currentTime <= 0)
        {
            StartTimer(false);
            health.PlayerDeath(true);
            playerDiedFromTime = true;
        }
        }
    }


    //for adaptation engine
    public float ReturnTimeLimit()
    {
        return timeLimit;
    }
    
    public float ReturnTimeTaken()
    {
        
        if (playerDiedFromTime)
        {
            return timeLimit - 1;
        }
        //returns amount of time remaining as a percentage of total time
        return timeLimit - currentTime; 
    }

    public void SetTimeLimit(float time)
    {
        timeLimit = time;
        currentTime = time;
        Debug.Log("SetTimeLimit called. New time is: "+time+"timeLimit variable = "+timeLimit);
    }
}