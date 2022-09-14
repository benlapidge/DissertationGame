using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpecialLoadEnemy : MonoBehaviour
{
    [SerializeField] GameObject weakEnemy;
    private Transform spawnPoint;
    void OnTriggerEnter(Collider other)
    {
        spawnPoint = GameObject.Find("SPAWN").transform;
        
        if (other.tag == "Player")
        {
            Instantiate(weakEnemy, new Vector3(spawnPoint.position.x,
                    spawnPoint.position.y + 1,
                    spawnPoint.position.z),
                spawnPoint.rotation);
            
            Destroy(this.gameObject);
        }
    }
}
