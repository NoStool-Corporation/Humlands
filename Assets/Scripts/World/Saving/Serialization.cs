﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

/// <summary>
/// A utility class to save and load chunks, entities, the camera and the seed
/// </summary>
public static class Serialization
{
    public static string saveFolderName = "Saves";
    public static string entityFileName = "Entities.bin";
    public static string cameraFileName = "Cam.luk";
    public static string   seedFileName = "seseseeeed.prt";
    private static Camera camera;

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
        SaveChunk save = new SaveChunk(chunk);
        if (save.blocks.Count == 0)
            return;

        string saveFile = SaveLocation(chunk.world.worldName);
        saveFile += FileName(chunk.pos);

        Save(saveFile, save);
    }
    // Saves Camera position and rotation.
    public static void SaveCam(string worldName)
    {
        if (!Directory.Exists(saveFolderName + "/" + worldName))
            return;

        SaveCamera camData = new SaveCamera(Camera.main);

        Save(saveFolderName + "/" + worldName + "/" + cameraFileName, camData);
    }
    // Saves the seed.
    public static void SaveSeed(string worldName, World world)
    {
        if (!Directory.Exists(saveFolderName + "/" + worldName))
            return;

        SaveSeed seedData = new SaveSeed(world);

        Save(saveFolderName + "/" + worldName + "/" + seedFileName, seedData);
    }
    /// <summary>
    /// Saves a List of Entities
    /// </summary>
    /// <param name="entities">The Linked List of Entities</param>
    public static void SaveEntities(List<Entity> entities, string worldName) {
        if (!Directory.Exists(saveFolderName + "/" + worldName))
            return;

        List<SaveEntity> save = new List<SaveEntity>();

        for (int i = 0; i < entities.Count; i++) {
            save.Add(new SaveEntity(entities[i]));
        }
        
        Save(saveFolderName + "/" + worldName + "/" + entityFileName, save);
    }

    /// <summary>
    /// Writes an object to file
    /// </summary>
    /// <param name="path">Path to the file (including the file name)</param>
    /// <param name="obj">The object</param>
    private static void Save(String path, object obj) {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, obj);
        stream.Close();
    }

    /// <summary>
    /// Loads all Entities
    /// </summary>
    /// <param name="entities">The reference to the List the Entities should be loaded into</param>
    /// <returns></returns>
    public static List<SaveEntity> LoadEntities(string worldName) {
        if (!File.Exists(saveFolderName + "/" + worldName + "/" + entityFileName))
            return null;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFolderName + "/" + worldName + "/" + entityFileName, FileMode.Open);

        List<SaveEntity> saves = (List<SaveEntity>) formatter.Deserialize(stream);

        stream.Close();

        return saves;
    }

    /// <summary>
    /// Loads saved blocks into the generated chunk
    /// </summary>
    /// <param name="chunk">An already generated chunk</param>
    /// <returns>Returns true if the load was successful, otherwise returns false</returns>
    public static bool LoadChunk(Chunk generatedChunk)
    {
        string saveFile = SaveLocation(generatedChunk.world.worldName);
        saveFile += FileName(generatedChunk.pos);

        if (!File.Exists(saveFile))
            return false;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        SaveChunk save = (SaveChunk)formatter.Deserialize(stream);
        foreach (var block in save.blocks)
        {
            generatedChunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            block.Value.SetupAfterSerialization();
        }
        stream.Close();
        return true;
    }

    // Deserializes Camera position and rotation.
    public static SaveCamera LoadCamera(string worldName)
    {
        if (!File.Exists(saveFolderName + "/" + worldName + "/" + cameraFileName))
            return null;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFolderName + "/" + worldName + "/" + cameraFileName, FileMode.Open);

        SaveCamera camData = (SaveCamera)formatter.Deserialize(stream);

        stream.Close();

        return camData;
    }
    // Deserializes seed
    public static SaveSeed LoadSeed(string worldName)
    {
        if (!File.Exists(saveFolderName + "/" + worldName + "/" + seedFileName))
            return null;

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFolderName + "/" + worldName + "/" + seedFileName, FileMode.Open);

        SaveSeed seedData = (SaveSeed)formatter.Deserialize(stream);

        stream.Close();

        return seedData;
    }
}