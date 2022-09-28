using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteToLogFile : MonoBehaviour
{

    public void CreateText(string text)
    {
        
        // path of file
        string textDocumentName = Application.persistentDataPath + "/participant_data/" + "send_this" + ".txt";
        // create file if doesnt exist
        if (!File.Exists(textDocumentName))
        {
            File.WriteAllText(textDocumentName,"Send this file to the researcher & enter contents into survey \n \nGAME VERSION IS \"TAU\"\n \n");
        }

        File.AppendAllText(textDocumentName, text);

    }
    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/participant_data/");
    }

}
