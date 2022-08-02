using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    private HealthSystem health;
    public bool startTimer;
    public float currentTime;
    [SerializeField] float timeLimit = 20.0f;

    private void Awake()
    {
        currentTime = timeLimit;
        health = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
    }
    
    private void Update()
    {
        StartTimer(startTimer);
    }

    public void StartTimer(bool activate)
    {
        if (activate)
        {
            Countdown();
        }
    }

    private void Countdown()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= timeLimit)
        {
            StartTimer(false);
            health.PlayerDeath();
        }
    }

    
    //for adaptation engine
    public float ReturnTimeLimit()
    {
        return timeLimit;
    }
    
    public float ReturnTimeTaken()
    {
        //returns amount of time remaining as a percentage of total time
        return (currentTime/timeLimit*100);
    }

    public void SetTimeLimit(float time)
    {
        timeLimit = time;
    }

    
}
