using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoadGame
{
    // Saves the GameData in a binary file using a binary formatter.
    public static void SaveGame(World world)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(Application.persistentDataPath + "/cantRead.this", FileMode.Create);

        GameData data = new GameData();

        formatter.Serialize(fs, data);
        fs.Close();
    }
        
    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/cantRead.this"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(Application.persistentDataPath + "cantRead.this", FileMode.Open);

            GameData data = formatter.Deserialize(fs) as GameData;
            fs.Close();
            // return data
        }
        else
        {
            Debug.LogError("Save does not exist.");
            return;
        }
    }
}


/* This part of the code has yet to be written.
 * It will contain the seed and everything that the Player changed while playing.
 * It will be written as soon as the Serialization process of changed Blocks can be implemented.


[Serializable]
public class GameData
{

}
*/



/* 
 * do /ignore
 * 
 * foreach (var chunk in chunks)
        {
            Destroy(chunk.Value);
chunks.Remove(chunk.Key);
            Serialization.SaveEntities(entities);
        }
*/