using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI keyList;
    [SerializeField] private TextMeshProUGUI timeLimit;
    [SerializeField] private TextMeshProUGUI gameQuit;
    [SerializeField] private GameObject button;
    [SerializeField] private InventorySystem inventory;
    [SerializeField] private Image damageTaken;
    [SerializeField] private Image healthTaken;
    private int currentKey;
    private AdaptationEngine engine = default;

    private void Start()
    {
        IncreaseHealth(100);
        gameQuit.enabled = false;
        button = GameObject.Find("Exit Game");
        button.SetActive(false);
        damageTaken.enabled = false;
        healthTaken.enabled = false;
        timeLimit.enabled = false;
        engine = GameObject.Find("AdaptationEngine").GetComponent<AdaptationEngine>();
    }

    private void Update()
    {
        currentKey = inventory.MaxKey();
        UpdateKeys(currentKey);
    }

    

    private void OnEnable()
    {
        GameEnding.OnEnd += EndGame;
        HealthSystem.OnDamage += ReduceHealth;
        HealthSystem.OnHeal += IncreaseHealth;
        HealthSystem.OnRepair += IncreaseHealth;
        HealthSystem.OnDeath += PlayerDeath;
        MetricSensor.OnTimerStart += TimerCountDown;

    }

    private void OnDisable()
    {
        GameEnding.OnEnd -= EndGame;
        HealthSystem.OnDamage -= ReduceHealth;
        HealthSystem.OnHeal -= IncreaseHealth;
        HealthSystem.OnRepair -= IncreaseHealth;
        HealthSystem.OnDeath -= PlayerDeath;
        MetricSensor.OnTimerStart -= TimerCountDown;
    }
    private void EndGame(bool end)
    {
        if (end)
        {
            gameQuit.enabled = true;
            button.SetActive(true);
        }
    }
    private void ReduceHealth(float crntHealth)
    {
        healthText.text = crntHealth.ToString("00");
        StartCoroutine(hitDamage());
    }

    private void IncreaseHealth(float crntHealth)
    {
        healthText.text = crntHealth.ToString("00");
        StartCoroutine(UpHealth());
    }

    private void PlayerDeath(bool dead)
    {
        if (dead) healthText.text = "50";
    }

    private void UpdateKeys(int crntKey)
    {
        keyList.text = crntKey.ToString("0");
    }
    private void TimerCountDown(float currentTime)
    {
        timeLimit.enabled = true;
        timeLimit.text = currentTime.ToString("00");
    }

    private IEnumerator hitDamage()
    {
        damageTaken.enabled = true;
        yield return new WaitForSeconds(0.2f);
        damageTaken.enabled = false;
    }

    private IEnumerator UpHealth()
    {
        healthTaken.enabled = true;
        yield return new WaitForSeconds(0.2f);
        healthTaken.enabled = false;
    }

    
}