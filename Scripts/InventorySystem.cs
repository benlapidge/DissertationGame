using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private readonly List<int> KeyList = new();


    public void SetKeys(int value)
    {
        KeyList.Add(value);
        foreach (var a in KeyList)
            Debug.Log("Key Added" + a);
    }

    public List<int> GetKeys()
    {
        return KeyList;
    }

    public int MaxKey()
    {
        if (KeyList.Count == 0)
            //Debug.Log("list empty");
            return 0;

        var maxKey = KeyList[KeyList.Count - 1];
        return maxKey;
    }
}