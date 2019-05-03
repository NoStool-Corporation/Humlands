using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Entity : MonoBehaviour
{
    public enum Jobs { Miner, Crafter, Baker }


    private string NAME_FILE_PATH = "Assets/Resources/Strings/EntityNames.txt";
    public static string PREFAB_PATH = "Prefabs/Entity";
    public int scanMapSize = 10;

    public Inventory inventory;
    public string entityName;
    public Jobs job;

    private World world;
	public bool isBreakingBlock;
    public bool isCraftingItem;

    public Movement movement = new Movement();

    public Entity()
    {
        isBreakingBlock = false;
        isCraftingItem = false;
        inventory = new Inventory(1000);
    }

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();

        WorldPos wp = FindNextBlockWithId(5);
        if (wp != null)
            movement.Start(transform.position, 2, wp.ToVector3());
    }
	
	void Update()
	{
        DoWork();
        if(movement.moving && !movement.paused)
            SetPosition(movement.GetPosition());
    }
    

    // Finds out what type of work the Entity is doing 
    // (breaking a block; crafting an item)
    // and calls the specific method created for that purpose.
    public void DoWork()
	{
        Block frontBlock = world.GetBlock(new WorldPos(transform.position + transform.forward.normalized));
        //print(frontBlock);
        //print(frontBlock as WorkTableBlock);
    	if (isBreakingBlock == false && frontBlock != null)
		{
            frontBlock.WorkOnToBreak();
		}
	    if (isCraftingItem == false && frontBlock as WorkTableBlock != null)
        {
            (frontBlock as WorkTableBlock).Craft(this);
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
        entityName = names[Random.Range(0, names.Length - 1)];
    }

    /// <summary>
    /// Tries to find the next Block having the id which is passed by the first argument.
    /// !Important at this stage of the method is that it only searches inside the chunks on y = 0!
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public WorldPos FindNextBlockWithId(uint id) {
        List<WorldPos> distances = new List<WorldPos>(LoadChunks.GetRenderDistanceChunks(10));

        //Get current Chunk Position of the Entity
        WorldPos currentChunkPos = new WorldPos();
        currentChunkPos.x = Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize;
        currentChunkPos.y = Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize;
        currentChunkPos.z = Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize;

        Debug.Log(currentChunkPos.x.ToString() + " | " + currentChunkPos.y.ToString() + " | " + currentChunkPos.z.ToString());

        //Tree Block-ID = 5
        WorldPos chunkPos;
        Chunk tmpCh;
        WorldPos blockPos = new WorldPos();
        bool stayLoadedBefore;

        for (int i = 0; i < distances.Count; i++)
        {
            //merge currentChunkPos and the chunk pos relative to the Entity
            chunkPos = distances[i];
            chunkPos.x = chunkPos.x * Chunk.chunkSize + currentChunkPos.x;
            chunkPos.y = 0;
            chunkPos.z = chunkPos.z * Chunk.chunkSize + currentChunkPos.z;

            tmpCh = world.BuildChunk(chunkPos);
            if (tmpCh == null)
                continue;

            stayLoadedBefore = tmpCh.stayLoaded;
            tmpCh.stayLoaded = true;
            blockPos = tmpCh.SearchBlock(id);


            if (blockPos == null) {
                tmpCh.stayLoaded = stayLoadedBefore;
                continue;
            }
        
            blockPos.x += tmpCh.pos.x;
            blockPos.y += tmpCh.pos.y;
            blockPos.z += tmpCh.pos.z;
            tmpCh.stayLoaded = stayLoadedBefore;
            
            return blockPos;
        }

        return null;
    }

    
    
}
