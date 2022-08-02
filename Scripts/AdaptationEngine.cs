using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdaptationEngine : MonoBehaviour
{
    [Header("Inputs")] [SerializeField] private MetricSensor room1;

    [SerializeField] private MetricSensor room2;
    [SerializeField] private MetricSensor room3;
    [SerializeField] private MetricSensor room4;
    [SerializeField] private MetricSensor room5;
    [SerializeField] private MetricSensor room6;
    [SerializeField] private MetricSensor room7;
    [SerializeField] private MetricSensor room8;
    [SerializeField] private MetricSensor room9;
    [SerializeField] private MetricSensor room10;
    //allows addition of all room sensors in engine.

    [Header("Instantiation Objects")] [SerializeField]
    private GameObject healthpack;

    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform nextroom;
    [SerializeField] private Transform playerRoom;


    private readonly List<MetricSensor> roomSensors = new();

    private int currentRoom;
    
    

    // Start is called before the first frame update
    private void Start()
    {
        //creates list of sensors for iteration

        roomSensors.Add(room1);
        roomSensors.Add(room2);
        roomSensors.Add(room3);
        roomSensors.Add(room4);
        roomSensors.Add(room5);
        roomSensors.Add(room6);
        roomSensors.Add(room7);
        roomSensors.Add(room8);
        roomSensors.Add(room9);
        roomSensors.Add(room10);
        nextroom = roomSensors[currentRoom + 1].transform;
        playerRoom = roomSensors[currentRoom].transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CheckHealth(currentRoom) >= 0)
        {
            Debug.Log("ADAPTATION ENGINE RECIEVED HEALTH LOSS OF " + CheckHealth(currentRoom) + " IN ROOM " +
                      currentRoom);

            AdaptHealth(CheckHealth(currentRoom));


            if (currentRoom <
                9) // this moves the algorithm to watch the next room, move this until after the last relevant method for the AE is completed.
            {
                currentRoom += 1;
                playerRoom = roomSensors[currentRoom].transform;
                nextroom = roomSensors[currentRoom+1].transform;
                Debug.Log("MOVING TO " + currentRoom);
            }
        }
    }


    private float CheckHealth(int a)
    {
        float enterHealth = 0;
        float exitHealth;
        var playerPresent = false;
        if (roomSensors[a].PlayerPresent())
        {
            playerPresent = true;
            enterHealth = roomSensors[a].EnterHealth();
        }

        if (playerPresent && roomSensors[a].PlayerExited())
        {
            exitHealth = roomSensors[a].ExitHealth();
            return enterHealth - exitHealth;
        }

        return -1;
    }

    private void AdaptHealth(float health)
    {
        //this is where the algorithm starts
        if (health >= 40)
        {
            Debug.Log("Called as health is VERY low");
            nextroom = nextroom.transform.GetChild(RandomSpawnPoint());
            Instantiate(healthpack, new Vector3(nextroom.position.x, nextroom.position.y+1, nextroom.position.z),
                nextroom.rotation);
        }
        else if (health >= 20)
        {
            Debug.Log("Called as health is low");
        }
        else if (health <= 10)
        {
            AdaptEnemy();
        }
    }

    private void AdaptEnemy()
    {
        Debug.Log("Called as game isn't tough enough");
        nextroom = nextroom.transform.GetChild(RandomSpawnPoint());
        Instantiate(enemy, new Vector3(nextroom.position.x, nextroom.position.y, nextroom.position.z),
            nextroom.rotation);
        Debug.Log("Enemy called");
    }


    private int RandomSpawnPoint()
    {
        //provides a random spawn point value when the number of spawn points within a room are counted
        return Random.Range(0, nextroom.transform.childCount - 1);
    }

    public Transform CurrentRoom()
    {
        return playerRoom;
    }
}