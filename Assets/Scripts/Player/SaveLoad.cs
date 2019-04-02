using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoad
{
    // Saves the game in a binary file using a formatter.
    public static void SaveGame(World world)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(Application.persistentDataPath + "/pleaseDoNot.look", FileMode.Create);

        GameData data = new GameData();

        formatter.Serialize(fs, data);
        fs.Close();
    }
        
    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/pleaseDoNot.look"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(Application.persistentDataPath + "/pleaseDoNot.look", FileMode.Open);

            GameData data = formatter.Deserialize(fs) as GameData;
            fs.Close();
            // return data.
        }
        else
        {
            Debug.LogError("save does not exist.");
            return;
        }
    }
}



[Serializable]
public class GameData
{

}

/* foreach (var chunk in chunks)
        {
            Destroy(chunk.Value);
chunks.Remove(chunk.Key);
            Serialization.SaveEntities(entities);
        }
*/