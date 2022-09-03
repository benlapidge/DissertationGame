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
    private AdaptationEngine engine = default;

    private void Awake()
    {
        engine = GameObject.Find("AdaptationEngine").GetComponent<AdaptationEngine>();
        player = GameObject.Find("Player").GetComponent<CharacterController>();
        gun = GameObject.Find("Gun").GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            List<Int32> playerScorelist = engine.GetPlayerScore();
            List<Int32> playerHealthScoreList = engine.GetPlayerHealthScore();
            List<Int32> playerTimeScoreList = engine.GetPlayerTimeScore();
            player.enabled = false; // stops player from moving
            gun.enabled = false;
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
