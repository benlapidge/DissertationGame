using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ADAPTATIONEEngine1 : MonoBehaviour
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

    [Header("Instantiation Objects")] 
    [SerializeField] private GameObject mediumHealthpack;
    [SerializeField] private GameObject largeHealthpack;

    [SerializeField] private GameObject weakEnemy;
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject toughEnemy;
    
    [Header("Debug")] 
    [SerializeField] private Transform nextRoom;
    [SerializeField] private Transform playerRoom;

//array of rooms
    private readonly List<MetricSensor> roomSensors = new();
//iterator for keeping track of which room player is in
    private int currentRoom;
    
    

    // Start is called before the first frame update
    private void Start()
    {
        //creates list of sensors for iteration and adds to array

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
        //sets initial values
        nextRoom = roomSensors[currentRoom + 1].transform;
        playerRoom = roomSensors[currentRoom].transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (MonitorHealth(roomSensors[currentRoom]) >= 0 && MonitorTime(roomSensors[currentRoom]) >= 0) //will likely struggle in edge cases where player loses no health but has time, will start running prematurely
        {
            // include iteration to next room once adaptation process is complete    
        
            if (currentRoom < 9) // this moves the algorithm to watch the next room, move this until after the last relevant method for the AE is completed.
            {
                currentRoom += 1;
                playerRoom = roomSensors[currentRoom].transform;
                nextRoom = roomSensors[currentRoom+1].transform;
                Debug.Log("MOVING TO " + currentRoom);
            }
        }
        
    }
    // Monitoring Methods
    private float MonitorHealth(MetricSensor room)
    {
        bool playerPresent = room.PlayerPresent();
        if (playerPresent && room.PlayerExited())
        {
            return room.EnterHealth() - room.ExitHealth();
        }
        return -1;
        
        
    }
    private float MonitorTime(MetricSensor room)
    {
        return 1.0f;
    }

    // Instantiation Methods

    private void SpawnEnemy(int value)
    {
        //TODO Fix this to work properly
        // this method takes in a value and then instantiates an enemy based on that value
        Transform EnemySpawnPoint = nextRoom.transform.GetChild(RandomSpawnPoint());

        switch (value)
        {
            case 0:
                Instantiate(normalEnemy, new Vector3(EnemySpawnPoint.position.x, EnemySpawnPoint.position.y+1, EnemySpawnPoint.position.z),
                    EnemySpawnPoint.rotation);
                break;
            
        }
    }

    private void RemoveEnemy(int value)
    {
        // this method removes a preset enemy from the subsequent room if called
    }

    private void SpawnHealthPack(int value)
    {
        //TODO Fix this to work properly
        Transform HealthPackSpawnPoint = nextRoom.transform.GetChild(RandomSpawnPoint());
        switch (value)
        {
            case 0:
                Instantiate(normalEnemy, new Vector3(HealthPackSpawnPoint.position.x, HealthPackSpawnPoint.position.y+1, HealthPackSpawnPoint.position.z),
                    HealthPackSpawnPoint.rotation);
                break;
            
        }
        // this method takes in a value and then instantiates a healthpack based on that value
    }
    
    private void RemoveHealthPack(int value)
    {
        // this method takes in a value and then instantiates a healthpack based on that value
    }
    
    // Alteration Methods

    private void AddTime(int value)
    {
        float time = 20;//TODO fix this to adjust based on conditions
        // This method adds a preset amount of time to the room timer in the next room
        roomSensors[currentRoom+1].SetTimeLimit(time);
    }
    
    private void ReduceTime(int value)
    {
        // This method removes a preset amount of time to the room timer in the next room
    }
    
    
    // for methods and spawning locations
    private int RandomSpawnPoint()
    {
        //provides a random spawn point value when the number of spawn points within a room are counted
        return Random.Range(0, nextRoom.transform.childCount - 1);
    }

    public Transform CurrentRoom()
    {
        return playerRoom;
    }
}