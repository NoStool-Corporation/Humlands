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
        movement.Start(transform.position, 1, new Vector3(transform.position.x + 100, transform.position.y, transform.position.z + 10));
        world = FindObjectOfType<World>();
    }
	
	void Update()
	{
        DoWork();

        transform.position = movement.GetPosition();
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
    public void SetPosition(WorldPos pos)
    {
        WorldPos newChPos = world.GetChunkPos(pos);
        WorldPos ownChPos = world.GetChunkPos(new WorldPos(transform.position));
        if (newChPos.Equals(ownChPos))
        {
            Chunk tmpCh = world.GetChunk(ownChPos);
            if (tmpCh != null)
                tmpCh.stayLoaded = false;

            tmpCh = world.BuildChunk(newChPos);
            if (tmpCh != null)
                tmpCh.stayLoaded = true;
        }

        transform.position = pos.ToVector3();
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
    /// Teleports the Entity relative to the current postion
    /// </summary>
    /// <param name="step"></param>
    public void Step(Vector3 step)
    {
        SetPosition(new WorldPos(transform.position + step));
    }
}
