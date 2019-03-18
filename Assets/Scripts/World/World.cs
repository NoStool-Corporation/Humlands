using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;
    public string worldName = "world";
    private int seed = 1;

    public int newChunkX;
    public int newChunkY;
    public int newChunkZ;

    public bool genChunk;

    private void Update()
    {

        if (genChunk)
        {
            genChunk = false;
            WorldPos chunkPos = new WorldPos(newChunkX, newChunkY, newChunkZ);
            Chunk chunk = null;

            if (chunks.TryGetValue(chunkPos, out chunk))
            {
                DestroyChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
            else
            {
                CreateChunk(chunkPos.x, chunkPos.y, chunkPos.z);
            }
        }
    }

    public void Start()
    {
        for (int x = -10; x < 10; x++)
        {
            for (int y = -3; y < 3; y++)
            {
                for (int z = -10; z < 10; z++)
                {
                    CreateChunk(x * 16 + 6400, y * 16, z * 16 + 64000);
                }
            }
        }

    }


    public void CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;

        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        chunks.Add(worldPos, newChunk);

        var terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk, seed);
        newChunk.SetBlocksUnmodified();
        bool loaded = Serialization.Load(newChunk);

        newChunk.SetBlocksUnmodified();
        Serialization.Load(newChunk);   

        newChunk.update = true;
        //apparently neighbor chunks already update?
        //UpdateNeighborChunks(newChunk);
    }
    
    /*public void UpdateNeighborChunks(Chunk midChunk)
    {
        WorldPos pos = midChunk.pos;
        Chunk c1 = GetChunk(pos.x + 16, pos.y, pos.z);
        Chunk c2 = GetChunk(pos.x, pos.y + 16, pos.z);
        Chunk c3 = GetChunk(pos.x, pos.y, pos.z + 16);
        Chunk c4 = GetChunk(pos.x - 16, pos.y, pos.z);
        Chunk c5 = GetChunk(pos.x, pos.y - 16, pos.z);
        Chunk c6 = GetChunk(pos.x, pos.y, pos.z - 16);

        if (c1 != null)
            c1.update = true;
        if (c2 != null)
            c2.update = true;
        if (c3 != null)
            c3.update = true;
        if (c4 != null)
            c4.update = true;
        if (c5 != null)
            c5.update = true;
        if (c6 != null)
            c6.update = true;
    }*/

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);
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

    public void SetBlock(int x, int y, int z, Block block, bool update = true)
    {
        Chunk chunk = GetChunk(x, y, z);
        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block, update);

            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }

    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }
}
