using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    private List<int> KeyList = new List<int>();
    

    public void SetKeys(int value)
    {
        KeyList.Add(value);
        foreach (int a in KeyList)
            Debug.Log("Key Added" + a);

    }

    public List<int> GetKeys()
    {
        return KeyList;
    }



}
