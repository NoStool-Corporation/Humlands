using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

/// <summary>
/// A utility class to save and load chunks
/// </summary>
public static class Serialization
{
    public static string saveFolderName = "Saves";
    /// <summary>
    /// Returns the save location based on the world name
    /// </summary>
    /// <param name="worldName"></param>
    /// <returns>Returns the path to the save folder</returns>
    public static string SaveLocation(string worldName)
    {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }
        return saveLocation;
    }
    /// <summary>
    /// Returns the file name based on the coordinates of the chunk
    /// </summary>
    /// <param name="chunkLocation"></param>
    /// <returns>Returns the file name</returns>
    public static string FileName(WorldPos chunkLocation)
    {
        string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
        return fileName;
    }
    /// <summary>
    /// Saves every block marked as changed of the specified chunk
    /// </summary>
    /// <param name="chunk"></param>
    public static void SaveChunk(Chunk chunk)
    {
        Save save = new Save(chunk);
        if (save.blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, save);
        stream.Close();

    }
    /// <summary>
    /// Loads saved blocks into the generated chunk
    /// </summary>
    /// <param name="chunk">An already generated chunk</param>
    /// <returns>Returns true if the load was successful, otherwise returns false</returns>
    public static bool Load(Chunk generatedChunk)
    {
        string saveFile = SaveLocation(generatedChunk.world.worldName);
        saveFile += FileName(generatedChunk.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        Save save = (Save)formatter.Deserialize(stream);
        foreach (var block in save.blocks)
        {
            generatedChunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        }
        stream.Close();
        return true;
    }
}