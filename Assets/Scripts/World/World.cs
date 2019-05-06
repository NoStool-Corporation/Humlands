using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The world in which the game takes place.
/// Has an array with all the loaded chunks,
/// has seed to determine how the world looks,
/// has a name to determine the save folder.
/// </summary>
public class World : MonoBehaviour
{
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public List<Entity> entities = new List<Entity>();
    GameObject chunkPrefab;
    GameObject entityPrefab;
    public string worldName = "world";
    private int seed = 1;
    TerrainGen terrainGen;
    public Dictionary<int, WorldPos> chunkPlaces = new Dictionary<int, WorldPos>();

    private void Start()
    {
        chunkPrefab = Resources.Load<GameObject>("Prefabs/Chunk");
        entityPrefab = Resources.Load<GameObject>(Entity.PREFAB_PATH);
        terrainGen = new TerrainGen(seed);
        LoadEntities();

        /*GameObject g = Instantiate(entityPrefab, new Vector3(6390.26f, 10f, 64052.61f), new Quaternion(0, 0, 0, 0));
        Entity e = g.GetComponent<Entity>();
        entities.Add(e);
        WorldPos pp = new WorldPos(30, 20, -5);
        entities[0].GiveRandomName();*/
    }

    // Saves world when player quits
    // through saving all chunk positions in the Dictionary "chunkPos"
    // and unloading the chunks with these positions afterwards
    // when the player closes the game.
    private void OnApplicationQuit()
    {
        int Key = 0;
        foreach (var chunk in chunks)
        { 
            WorldPos chunkPos = new WorldPos(chunk.Key.x, chunk.Key.y, chunk.Key.z);
            chunkPlaces.Add(Key, chunkPos);
            Key++;
        }
        foreach (var chunk in chunkPlaces)
        {
            UnloadChunk(chunk.Value.x, chunk.Value.y, chunk.Value.z);
        }
        Serialization.SaveEntities(entities, worldName);
    }

    /// <summary>  
    /// Creates a chunk with the smallest corner at the position
    /// </summary>
    ///
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// 
    /// <returns> Created Chunk </returns>
    public Chunk CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);
        if (chunks.ContainsKey(worldPos))
            return chunks[worldPos];

        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero));

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        chunks.Add(worldPos, newChunk);

        newChunk = terrainGen.ChunkGen(newChunk);
        newChunk.SetBlocksUnmodified();
        Serialization.LoadChunk(newChunk);

        return newChunk;
    }

    void LoadEntities()
    {
        List<SaveEntity> saves = Serialization.LoadEntities(worldName);
        if (saves == null)
        {
            print("No entities loadable");
            return;
        }


        GameObject gameObject;
        Entity e;
        Vector3 pos;
        Quaternion quat;

        foreach (SaveEntity save in saves)
        {
            pos = new Vector3(save.position[0], save.position[1], save.position[2]);
            quat = new Quaternion(save.rotation[0], save.rotation[1], save.rotation[2], save.rotation[3]);

            gameObject = Instantiate(entityPrefab, pos, quat);
            e = gameObject.GetComponent<Entity>();
            e.entityName = save.entityName;
            e.transform.position = pos;
            e.transform.rotation = quat;
            e.inventory = save.inventory;
            e.job = save.job;
            entities.Add(e);
        }

    }

    /// <summary>  
    /// Creates a chunk with the smallest corner at the position
    /// </summary>
    /// 
    /// <param name="worldpos"></param>
    /// 
    /// <returns> created Chunk </returns>
    public Chunk CreateChunk(WorldPos worldpos)
    {
        return CreateChunk(worldpos.x, worldpos.y, worldpos.z);
    }

    /// <summary>  
    /// Returns the chunk at the specified position
    /// </summary>
    /// 
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// 
    /// <returns> Chunk at x,y,z </returns>
    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = GetChunkPos(x,y,z);

        if (!chunks.ContainsKey(pos)) {
            return null;
        }
            

        return chunks[pos];
    }

    /// <summary>
    /// Creates a chunk if there isn't alreay one
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>Retuns the created chunk or null if there already was one</returns>
    public Chunk BuildChunk(WorldPos pos)
    {
        if (GetChunk(pos.x, pos.y, pos.z) == null)
            return CreateChunk(pos.x, pos.y, pos.z);
        return GetChunk(pos);
    }

    /// <summary>  
    /// Returns the chunk at the specified position
    /// </summary>
    /// 
    /// <param name="worldpos"></param>
    /// 
    /// <returns> Chunk at x,y,z </returns>
    public Chunk GetChunk(WorldPos worldpos)
    {
        return GetChunk(worldpos.x, worldpos.y, worldpos.z);
    }
    /// <summary>
    /// Returns the block at the specified position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns> Returns the block at the specified position or AirBlock if no block exists </returns>
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x,y,z);

        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);
            return block;
        }
        else
        {
            return new AirBlock();
        }

    }
    /// <summary>
    /// Returns the block at the specified position
    /// </summary>
    /// <param name="worldpos"></param>
    /// <returns> Returns the block at the specified position or AirBlock if no block exists </returns>
    public Block GetBlock(WorldPos worldpos)
    {
        if (worldpos == null)
            return null;

        return GetBlock(worldpos.x, worldpos.y, worldpos.z);
    }
    /// <summary>
    /// Places a block at the specified position.
    /// </summary>
    /// <param name="worldpos"></param>
    /// <param name="block"></param>
    /// <param name="render">Rerenders the chunk if set to true, defaults to true</param>
    public void SetBlock(WorldPos worldpos, Block block, bool render = true)
    {
        SetBlock(worldpos.x, worldpos.y, worldpos.z, block, render);
    }
    /// <summary>
    /// Places a block at the specified position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="block"></param>
    /// <param name="render">Rerenders the chunk if set to true, defaults to true</param>
    public void SetBlock(int x, int y, int z, Block block, bool render = true)
    {
        Chunk chunk = GetChunk(x, y, z);
        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block, render);

            RenderIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            RenderIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            RenderIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            RenderIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            RenderIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            RenderIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }
    /// <summary>
    /// Unloads and saves the chunk at the specified position in the chunk grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void UnloadChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    /// <summary>
    /// Unloads and saves the chunk at the specified position in the chunk grid
    /// </summary>
    /// <param name="pos"></param>
    public void UnloadChunk(WorldPos pos)
    {
        UnloadChunk(pos.x, pos.y, pos.z);
    }

    /// <summary>
    /// Returns the coordinate of the Chunk in the Chunk grid at a specified position inside the world coordiante system
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public WorldPos GetChunkPos(WorldPos pos)
    {
        WorldPos ret = new WorldPos();
        float multiple = Chunk.chunkSize;
        ret.x = Mathf.FloorToInt(pos.x / multiple) * Chunk.chunkSize;
        ret.y = Mathf.FloorToInt(pos.y / multiple) * Chunk.chunkSize;
        ret.z = Mathf.FloorToInt(pos.z / multiple) * Chunk.chunkSize;
        return ret;
    }

    /// <summary>
    /// Returns the coordinate of the Chunk in the Chunk grid at a specified position inside the world coordiante system
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public WorldPos GetChunkPos(int x, int y, int z)
    {
        return GetChunkPos(new WorldPos(x, y, z));
    }

    /// <summary>
    /// Renders the chunk at if the values are equal,
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <param name="pos">WorldPos of the chunk to render</param>
    void RenderIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos);
            if (chunk != null)
                chunk.render = true;
        }
    }

    public static float CalculateDistance(Vector3 a, Vector3 b) {
        a -= b;
        return Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
    }
}