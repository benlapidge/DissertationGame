using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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

    [Header("Instantiation Objects")]
    [SerializeField] private GameObject largeHealthpack;

    [SerializeField] private GameObject weakEnemy;
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject toughEnemy;
    
    [Header("Debug")] 
    [SerializeField] private Transform nextRoom;
    [SerializeField] private Transform playerRoom;

//array of rooms
    private readonly List<MetricSensor> roomSensors = new();
    private readonly List<Int32> playerScore = new();
//iterator for keeping track of which room player is in
    private int currentRoom;
    private int healthScore;
    private int timeScore;
    
    

    // Start is called before the first frame update
    public void Start()
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
        AdaptHealth(MonitorHealth(roomSensors[currentRoom]));
        AdaptTime(MonitorTime(roomSensors[currentRoom]));
        playerScore.Add(AddScores(healthScore, timeScore));
        
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
    public float MonitorHealth(MetricSensor room)
    {
        bool playerPresent = room.PlayerPresent();
        if (playerPresent && room.PlayerExited())
        {
            Debug.Log("AD HL +" + (room.EnterHealth() - room.ExitHealth()));
            return room.EnterHealth() - room.ExitHealth(); //todo test this
        }
        return -1;
        
        
    }
    public float MonitorTime(MetricSensor room)
    {
        
        bool playerPresent = room.PlayerPresent();
        if (playerPresent && room.PlayerExited())
        {
            Debug.Log("AE Monitor Time  +" + (room.ReturnTimeTaken() / room.ReturnTimeLimit() * 100));
            return room.ReturnTimeTaken() / room.ReturnTimeLimit() * 100; // returns time used as percentage
        }
        return -1;
    }
// Adaptation Methods & Randomizer
    public void AdaptHealth(float healthLoss)
    {
        int randomizer = Random.Range(0, 3);
        Debug.Log("Random digit is: "+randomizer);
        if (healthLoss >= 50) //low health state
        {
            Debug.Log("AE AdaptHealth LOW state");
            // TODO random value for HIGH health loss. Either fewer enemies, weaker enemies, or mega health pack
            switch (randomizer)
            {
                case 0:
                    SpawnHealthPack();
                    Debug.Log("AE AdaptHealth LOW state SpawnHealthPack");
                    break;
                case 1:
                    ReplaceEnemy(0);
                    Debug.Log("AE AdaptHealth LOW state ReplaceEnemy 0");
                    break;
                case 2:
                    RemoveEnemy();
                    Debug.Log("AE AdaptHealth LOW state RemoveEnemy 0");
                    break;
            }
            healthScore = 1;//adds score of 1 "low state"
            
        } else if (healthLoss >= 26) //medium health state
        {
            //does nothing for this health state
            Debug.Log("AE AdaptHealth MEDIUM state");
            healthScore = 2;//adds score of 2 "medium state"
            
        } else if (healthLoss <= 25) //high health state
        {
            Debug.Log("AE AdaptHealth HIGH state");
            // TODO random value for HIGH health loss. Either fewer enemies, weaker enemies, or mega health pack
            switch (randomizer)
            {
                case 0:
                    RemoveHealthPack();
                    Debug.Log("AE AdaptHealth HIGH state RemoveHealthPack");
                    break;
                case 1:
                    ReplaceEnemy(1);
                    Debug.Log("AE AdaptHealth HIGH state ReplaceEnemy 1");
                    break;
                case 2:
                    SpawnEnemy();
                    Debug.Log("AE AdaptHealth HIGH state SpawnEnemy");
                    break;
            }
            healthScore = 3;//adds score of 3 "high state"
            
        }
    }
    public void AdaptTime(float timeUsed)
    {
        int randomizer = Random.Range(0, 3); // todo return random number and check they are different
        if (timeUsed >= 50) // slow time state
        {
            Debug.Log("AE AdaptTime SLOW state");
            switch (randomizer)
            {
                case 0:
                    AddTime(timeUsed);
                    Debug.Log("AE AdaptTime SLOW state AddTime");
                    break;
                case 1:
                    ReplaceEnemy(0);
                    Debug.Log("AE AdaptTime SLOW state ReplaceEnemy 0");
                    break;
                case 2:
                    RemoveEnemy();
                    Debug.Log("AE AdaptTime SLOW state RemoveEnemy");
                    break;
            }
            timeScore = 1;//adds score of 1 "low state"
            
        } else if (timeUsed >= 11) //medium health state
        {
            //does nothing for this health state
            Debug.Log("AE AdaptTime NORMAL state");
            timeScore = 2;//adds score of 2 "medium state"
            
        } else if (timeUsed <= 10) //high health state
        {
            Debug.Log("AE AdaptTime FAST state");
            // TODO random value for HIGH health loss. Either fewer enemies, weaker enemies, or mega health pack
            switch (randomizer)
            {
                case 0:
                    ReduceTime(timeUsed);
                    Debug.Log("AE AdaptTime FAST state ReduceTime");
                    break;
                case 1:
                    ReplaceEnemy(1);
                    Debug.Log("AE AdaptTime FAST state ReplaceEnemy 1");
                    break;
                case 2:
                    SpawnEnemy();
                    Debug.Log("AE AdaptTime FAST state SpawnEnemy");
                    break;
            }
            timeScore = 3;//adds score of 3 "high state"
            
        }
    }

    public int AddScores(int healthscore, int timescore)
    {
        return healthscore + timescore;
    }

    private List<Int32> GetPlayerScore()
    {
        return playerScore; // for UI to display score at end
    }
    // Instantiation Methods

    public void SpawnEnemy()
    {
        Debug.Log("AE SpawnEnemy");
        Transform EnemySpawnPoint = nextRoom.transform.Find("SpawnPoints").GetChild(RandomSpawnPoint());
        Instantiate(normalEnemy, new Vector3(EnemySpawnPoint.position.x, EnemySpawnPoint.position.y+1, EnemySpawnPoint.position.z),
                    EnemySpawnPoint.rotation);
        
    }

    public void ReplaceEnemy(int value)
    {
        // if 0 replace normal enemy with weaker enemy
        // if 1 replace normal enemy with stronger enemy
        // this method replaces a preset enemy from the subsequent room if called
        Debug.Log("AE ReplaceEnemy");
        
        int childCountEnemiesInNextRoom = nextRoom.transform.Find("Enemies").childCount; // counts number of children within the enemy gameobject
        int childCountRandom = Random.Range(0, childCountEnemiesInNextRoom); //  random child within this gameobject

        GameObject randomEnemyInNextRoom = nextRoom.transform.Find("Enemies").GetChild(childCountRandom).gameObject; // selects the gameobject (enemy) ready for deletion
        Transform randomEnemyInNextRoomPosition = nextRoom.transform.Find("Enemies").GetChild(childCountRandom); // accesses the position of the enemy and stores it
        if (value == 0)
        {
            if (randomEnemyInNextRoom.name == "ToughEnemy") // checks enemy type to reduce
            {
                Destroy(randomEnemyInNextRoom);
                Instantiate(normalEnemy, new Vector3(randomEnemyInNextRoomPosition.position.x, randomEnemyInNextRoomPosition.position.y, randomEnemyInNextRoomPosition.position.z),
                    randomEnemyInNextRoomPosition.rotation); // instantiates the lower tier of enemy at the position of the original enemy
            } else if (randomEnemyInNextRoom.name == "NormalEnemy") // checks enemy type to reduce
            {
                Destroy(randomEnemyInNextRoom);
                Instantiate(weakEnemy, new Vector3(randomEnemyInNextRoomPosition.position.x, randomEnemyInNextRoomPosition.position.y, randomEnemyInNextRoomPosition.position.z),
                    randomEnemyInNextRoomPosition.rotation); // instantiates the lower tier of enemy at the position of the original enemy
            } else if (randomEnemyInNextRoom.name == "WeakEnemy") // checks enemy type to reduce
            {
                
                Debug.Log("no replacement made"); //todo iterate to other child if this occurs
            }
            
        }
        if (value == 1)
        {
            if (randomEnemyInNextRoom.name == "ToughEnemy") // checks enemy type to reduce
            {
                Debug.Log("no replacement made"); // instantiates the lower tier of enemy at the position of the original enemy
                
            } else if (randomEnemyInNextRoom.name == "NormalEnemy") // checks enemy type to reduce
            {
                Destroy(randomEnemyInNextRoom);
                Instantiate(toughEnemy, new Vector3(randomEnemyInNextRoomPosition.position.x, randomEnemyInNextRoomPosition.position.y, randomEnemyInNextRoomPosition.position.z),
                    randomEnemyInNextRoomPosition.rotation); // instantiates the lower tier of enemy at the position of the original enemy
            } else if (randomEnemyInNextRoom.name == "WeakEnemy") // checks enemy type to reduce
            {
                
                Destroy(randomEnemyInNextRoom);
                Instantiate(normalEnemy, new Vector3(randomEnemyInNextRoomPosition.position.x, randomEnemyInNextRoomPosition.position.y, randomEnemyInNextRoomPosition.position.z),
                    randomEnemyInNextRoomPosition.rotation);
            }
            
        }
    }
    public void RemoveEnemy()
    {
        // this method removes a preset enemy from the subsequent room if called
        Debug.Log("AE RemoveEnemy");
        int childCountEnemiesInNextRoom = nextRoom.transform.Find("Enemies").childCount; // counts number of children within the enemy gameobject
        int childCountRandom = Random.Range(0, childCountEnemiesInNextRoom); //  random child within this gameobject

        GameObject randomEnemyInNextRoom = nextRoom.transform.Find("Enemies").GetChild(childCountRandom).gameObject; // selects the gameobject (enemy) ready for deletion
        Destroy(randomEnemyInNextRoom);
    }

    public void SpawnHealthPack()
    {
        Debug.Log("AE SpawnHealthPack");
        Transform HealthPackSpawnPoint = nextRoom.transform.Find("SpawnPoints").GetChild(RandomSpawnPoint());
        Instantiate(largeHealthpack, new Vector3(HealthPackSpawnPoint.position.x, HealthPackSpawnPoint.position.y+1, HealthPackSpawnPoint.position.z),
                    HealthPackSpawnPoint.rotation);
    }
    
    public void RemoveHealthPack()
    {
        // removes a random healthpack if present in level
        Debug.Log("AE RemoveHealthPack");
        int childCountHealthPacksInNextRoom = nextRoom.transform.Find("HealthPacks").childCount; // counts number of children within the health gameobject
        int childCountRandom = Random.Range(0, childCountHealthPacksInNextRoom); //  random child within this gameobject

        GameObject randomHealthPackInNextRoom = nextRoom.transform.Find("HealthPacks").GetChild(childCountRandom).gameObject; // selects the gameobject (healthpack) ready for deletion
        Destroy(randomHealthPackInNextRoom);
        
    }
    
    // Alteration Methods

    public void AddTime(float value)
    {
        
        
        float newTime = roomSensors[currentRoom].ReturnTimeLimit()+(value/100)/2*roomSensors[currentRoom].ReturnTimeLimit();
        // takes half of the percentage of time used in the previous room, and finds how much time this was as part of the original time limit, then adds it to the original time limit, then sets the new time limit as this number
        // This method adds a preset amount of time to the room timer in the next room
        
        
        roomSensors[currentRoom+1].SetTimeLimit(newTime);
        Debug.Log("AE AddTime "+ newTime);
    }
    
    public void ReduceTime(float value)
    {
        // This method removes a preset amount of time to the room timer in the next room
        float newTime = roomSensors[currentRoom].ReturnTimeLimit()-(value/100)/2*roomSensors[currentRoom].ReturnTimeLimit();
        
        // takes half of the percentage of time used in the previous room, and finds how much time this was as part of the original time limit, then subtracts it to the original time limit, then sets the new time limit as this number
        // This method adds a preset amount of time to the room timer in the next room
        
        roomSensors[currentRoom+1].SetTimeLimit(newTime);
        Debug.Log("AE ReduceTime next toom time is "+ newTime);
    }
    
    
    // for methods and spawning locations
    public int RandomSpawnPoint()
    {
        //provides a random spawn point value when the number of spawn points within a room are counted
        return Random.Range(0, nextRoom.transform.Find("SpawnPoints").childCount);
    }

    public Transform CurrentRoom()
    {
        return playerRoom;
    }
}