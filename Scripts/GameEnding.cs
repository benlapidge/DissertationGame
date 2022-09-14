using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    // Start is called before the first frame update
    private WriteToLogFile writer = new();
    private CharacterController player = default;
    private MeshRenderer gun = default;
    private MeshRenderer barrel = default;
    private AdaptationEngine engine = default;
    public static Action<bool> OnEnd;
    private void Awake()
    {
        engine = GameObject.Find("AdaptationEngine").GetComponent<AdaptationEngine>();
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        gun = GameObject.Find("Gun").GetComponent<MeshRenderer>();
        barrel = GameObject.Find("Barrel").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnEnd?.Invoke(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            List<Int32> playerScorelist = engine.GetPlayerScore();
            List<Int32> playerHealthScoreList = engine.GetPlayerHealthScore();
            List<Int32> playerTimeScoreList = engine.GetPlayerTimeScore();
            player.enabled = false; // stops player from moving
            gun.enabled = false;
            barrel.enabled = false;
            writer.CreateText("PS:\n");
            foreach (int score in playerScorelist)
            {
                writer.CreateText(score.ToString());
            }
            
            writer.CreateText("\n\n");
            writer.CreateText("PHS:\n");
            
            foreach (int score in playerHealthScoreList)
            {
                writer.CreateText(score.ToString());
            }
            
            writer.CreateText("\n\n");
            writer.CreateText("PTS:\n");
            
            foreach (int score in playerTimeScoreList)
            {
                writer.CreateText(score.ToString());
            }
        }
    }

    
}
