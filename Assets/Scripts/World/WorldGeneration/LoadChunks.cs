using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LoadChunks : MonoBehaviour
{
    World world;

    List<WorldPos> renderList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    private WorldPos[] chunkPositions = null;
    
    int renderHeight = 3;

    public uint chunksPerTick;
    public int renderDistance = 8;
    private int blockRenderDistance = -1;

    /// <summary>
    /// Timer to only unload chunks every 10 update ticks
    /// </summary>
    int timer = 0;

    private void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
        chunkPositions = GetRenderDistanceChunks();
    }

    void Update()
    {
        UnloadChunks();
        FindChunksToLoad();
        LoadAndRenderChunks();
    }
    /// <summary>
    /// Finds all the chunks which need to be loaded but aren't yet and adds them to the render and build lists.
    /// Also builds surrounding chunks, but doesn't render them.
    /// </summary>
    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
            );

        if(chunkPositions == null)
            chunkPositions = GetRenderDistanceChunks();

        if (renderList.Count == 0)
        {
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPos newChunkPos = new WorldPos(chunkPositions[i].x * Chunk.chunkSize + playerPos.x, 0, chunkPositions[i].z * Chunk.chunkSize + playerPos.z);

                Chunk newChunk = world.GetChunk(
                    newChunkPos.x, newChunkPos.y, newChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null && (newChunk.rendered || renderList.Contains(newChunkPos)))
                    continue;

                //load a column of chunks in this position
                for (int y = 0; y < renderHeight; y++)
                {
                    for (int x = newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                    {
                        for (int z = newChunkPos.z - Chunk.chunkSize; z <= newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                        {
                            buildList.Add(new WorldPos(x, y * Chunk.chunkSize, z));
                        }
                    }
                    renderList.Add(new WorldPos(newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                }
                return;
            }
        }
    }

    /// <summary>
    /// Loads all the chunks in the build list and renders all the chunks in the render list
    /// </summary>
    void LoadAndRenderChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < chunksPerTick; i++)
            {
                Chunk chunk = world.BuildChunk(buildList[0]);
                if (chunk != null)
                    chunk.renderNeighbors = true;

                buildList.RemoveAt(0);
            }
            return;
        }
        if (renderList.Count != 0)
        {
            Chunk chunk = world.GetChunk(renderList[0].x, renderList[0].y, renderList[0].z);
            if (chunk != null)
            {
                chunk.render = true;
            }
            renderList.RemoveAt(0);
        }
    }
    /// <summary>
    /// Unloads chunks that are far away from the player
    /// </summary>
    /// <returns>Returns true if any chunks were unloaded, otherwise returns false</returns>
    bool UnloadChunks()
    {
        if (timer == 10)
        {
            bool deleted = false;
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));
                if (!IsInRenderDistance(chunk.Value.pos, false) && !chunk.Value.stayLoaded)
                    chunksToDelete.Add(chunk.Key);
            }
            foreach (var chunk in chunksToDelete)
            {
                world.UnloadChunk(chunk.x, chunk.y, chunk.z);
                deleted = true;
            }
            timer = 0;
            return deleted;
        }
        timer++;
        return false;
    }

    public bool IsInRenderDistance(WorldPos pos, bool isChunkPos)
    {
        if (blockRenderDistance < 0)
            blockRenderDistance = renderDistance * 16;

        if (isChunkPos)
        {
            pos.x *= 16;
            pos.y *= 16;
            pos.z *= 16;
        }

        if (Mathf.Abs(pos.x - transform.position.x) <= blockRenderDistance && Mathf.Abs(pos.z - transform.position.z) <= blockRenderDistance)
            return true;

        return false;
    }

    /// <summary>
    /// Generates an array of all chunks inside a 45° rotated square arround the camera
    /// </summary>
    /// <returns></returns>
    public static WorldPos[] GetRenderDistanceChunks(int renderDistance) {
        if (renderDistance < 1)
        {
            Debug.Log(renderDistance);
            return null;
        }

        WorldPos[] ret = new WorldPos[(renderDistance - 1) * renderDistance * 2 + 1];
        int i = 0;

        ret[i] = new WorldPos(0, 0, 0);
        i++;

        for (int z = 1; z < renderDistance; z++)
        {
            for (int x = 0; x < z; x++)
            {
                ret[i] = new WorldPos(x, 0, z - x);
                i++;
                ret[i] = new WorldPos(ret[i - 1].z, 0, ret[i - 1].x * -1);
                i++;
                ret[i] = new WorldPos(ret[i - 2].x * -1, 0, ret[i - 2].z * -1);
                i++;
                ret[i] = new WorldPos(ret[i - 3].z * -1, 0, ret[i - 3].x);
                i++;
            }
        }
        return ret;
    }

    /// <summary>
    /// Generates an array of all chunks inside a 45° rotated square arround the camera
    /// </summary>
    /// <returns></returns>
    public WorldPos[] GetRenderDistanceChunks() {
        return GetRenderDistanceChunks(renderDistance);
    }

    /// <summary>
    /// Updates the render distance and creates a new "nearby-chunks-square"
    /// </summary>
    /// <param name="r"></param>
    public void UpdateRenderDistance(int r) {
        renderDistance = r;
        chunkPositions = GetRenderDistanceChunks();
    }
}