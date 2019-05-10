using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class Entity : MonoBehaviour
{
    public enum Jobs { Miner, Crafter, Baker }


    public static string NAME_FILE_PATH = "Assets/Resources/Strings/EntityNames.txt";
    public static string PREFAB_PATH = "Prefabs/Entity";
    public int scanMapSize = 10;

    public Inventory inventory;
    public string entityName;
    public Jobs job;

    private World world;
	public bool isBreakingBlock;
    public bool isCraftingItem;
    public bool isBuildingBlock;

    bool foundWorktable = false;

    public Movement movement = new Movement();

    public Entity()
    {
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isBreakingBlock = false;
        isCraftingItem = true;
        isBuildingBlock = false;
        inventory = new Inventory(1000);
        world = FindObjectOfType<World>();
        //new InventoryUIManager(inventory);
    }

    private void Update()
    {
        if (!foundWorktable)
        {
            WorldPos wp = FindNextBlockOfType(typeof(CarpenterBlock));

            if (wp == null)
                return;

            movement.Start(transform.position, 10, wp.ToVector3() + new Vector3(0, -0.5f, -1));
            foundWorktable = true;
        }
       
        if (movement.moving && !movement.paused)
        {
            SetPosition(movement.GetPosition());
        }
    }

    private void FixedUpdate()
    {
        DoWork();
    }


    // Finds out what type of work the Entity is doing 
    // (breaking a block; crafting an item)
    // and calls the specific method created for that purpose.
    public void DoWork()
	{
        Block frontBlock = world.GetBlock(new WorldPos(transform.position + new Vector3(0,0.5f,1)));
    	if (isBreakingBlock == true && frontBlock != null)
		{
            frontBlock.WorkOnToBreak();
		}
	    if (isCraftingItem == true && frontBlock is WorkTableBlock)
        {
            ((WorkTableBlock) frontBlock).Craft(this);
        }
        if (isBuildingBlock == true && frontBlock as BlueprintBlock != null)
        {
            (frontBlock as BlueprintBlock).WorkOnToBuild(world, WorldPos.ToWorldPos(transform.position + transform.forward.normalized));
        }
    }
	
    /// <summary>
    /// Sets the position of the Entity; Loads the new Chunk and Uloads the old one
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector3 pos)
    {
		WorldPos wp = new WorldPos(transform.position);
        WorldPos newChPos = world.GetChunkPos(new WorldPos(pos));
        WorldPos ownChPos = world.GetChunkPos(wp);

        if (newChPos.Equals(ownChPos))
        {
            Chunk tmpCh = world.GetChunk(ownChPos);
            if (tmpCh != null)
                tmpCh.stayLoaded = false;

            tmpCh = world.BuildChunk(newChPos);
            if (tmpCh != null)
                tmpCh.stayLoaded = true;
            //tmpCh.render = true;
        }

        transform.position = pos;
    }

    /// <summary>
    /// Parses the names of the name file by splitting the string after every new line
    /// </summary>
    /// <returns>stirng[] of all the names form the name file</returns>
    private string[] GetNames()
    {
        StreamReader reader = new StreamReader(NAME_FILE_PATH);
        string read = reader.ReadToEnd();
        reader.Close();

        string[] ret = read.Split('\n');

        return ret;
    }

    /// <summary>
    /// Chooses a random name for the entity 
    /// </summary>
    public void GiveRandomName()
    {
        string[] names = GetNames();
        entityName = names[UnityEngine.Random.Range(0, names.Length - 1)];
    }

    /// <summary>
    /// Tries to find the next Block having the id which is passed by the first argument.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public WorldPos FindNextBlockOfType(Type t) {
        List<WorldPos> distances = new List<WorldPos>(LoadChunks.GetRenderDistanceChunks(5));

        //Get current Chunk Position of the Entity
        WorldPos currentChunkPos = world.GetChunkPos(new WorldPos(transform.position));

        //Tree Block-ID = 5
        WorldPos chunkPos = new WorldPos();
        Chunk tmpCh;
        WorldPos[] blocksPos;
        bool stayLoadedBefore;

        for (int i = 0; i < distances.Count; i++)
        {
            for (int y = 0; y < 3; y++)
            {

                chunkPos.x = (distances[i].x * Chunk.chunkSize) + currentChunkPos.x;
                chunkPos.y = y * Chunk.chunkSize;
                chunkPos.z = (distances[i].z * Chunk.chunkSize) + currentChunkPos.z;

                tmpCh = world.BuildChunk(chunkPos);
                if (tmpCh == null)
                    continue;

                stayLoadedBefore = tmpCh.stayLoaded;
                tmpCh.stayLoaded = true;
                blocksPos = tmpCh.GetBlocksOfType(t);
                tmpCh.stayLoaded = stayLoadedBefore;


                if (blocksPos == null)
                    continue;

                int smallest = 0;
                float smallestDist = World.CalculateDistance(transform.position, blocksPos[0].ToVector3());
                for (int j = 1; j < blocksPos.Length; j++) {
                    if (World.CalculateDistance(transform.position, blocksPos[j].ToVector3()) < smallestDist)
                        smallest = j;
                }

                

                return blocksPos[smallest];
            }
        }

        return null;
    }
    
}
