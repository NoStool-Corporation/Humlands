using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LoadChunks : MonoBehaviour
{
    World world;

    List<WorldPos> renderList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();
    /// <summary>
    /// TEMPORARY ridiculous array of chunks to load around the player, send help
    /// </summary>
    static WorldPos[] chunkPositions= {    new WorldPos( 0, 0,  0), new WorldPos(-1, 0,  0), new WorldPos( 0, 0, -1), new WorldPos( 0, 0,  1), new WorldPos( 1, 0,  0),
                                            new WorldPos(-1, 0, -1), new WorldPos(-1, 0,  1), new WorldPos( 1, 0, -1), new WorldPos( 1, 0,  1), new WorldPos(-2, 0,  0),
                                            new WorldPos( 0, 0, -2), new WorldPos( 0, 0,  2), new WorldPos( 2, 0,  0), new WorldPos(-2, 0, -1), new WorldPos(-2, 0,  1),
                                            new WorldPos(-1, 0, -2), new WorldPos(-1, 0,  2), new WorldPos( 1, 0, -2), new WorldPos( 1, 0,  2), new WorldPos( 2, 0, -1),
                                            new WorldPos( 2, 0,  1), new WorldPos(-2, 0, -2), new WorldPos(-2, 0,  2), new WorldPos( 2, 0, -2), new WorldPos( 2, 0,  2),
                                            new WorldPos(-3, 0,  0), new WorldPos( 0, 0, -3), new WorldPos( 0, 0,  3), new WorldPos( 3, 0,  0), new WorldPos(-3, 0, -1),
                                            new WorldPos(-3, 0,  1), new WorldPos(-1, 0, -3), new WorldPos(-1, 0,  3), new WorldPos( 1, 0, -3), new WorldPos( 1, 0,  3),
                                            new WorldPos( 3, 0, -1), new WorldPos( 3, 0,  1), new WorldPos(-3, 0, -2), new WorldPos(-3, 0,  2), new WorldPos(-2, 0, -3),
                                            new WorldPos(-2, 0,  3), new WorldPos( 2, 0, -3), new WorldPos( 2, 0,  3), new WorldPos( 3, 0, -2), new WorldPos( 3, 0,  2),
                                            new WorldPos(-4, 0,  0), new WorldPos( 0, 0, -4), new WorldPos( 0, 0,  4), new WorldPos( 4, 0,  0), new WorldPos(-4, 0, -1),
                                            new WorldPos(-4, 0,  1), new WorldPos(-1, 0, -4), new WorldPos(-1, 0,  4), new WorldPos( 1, 0, -4), new WorldPos( 1, 0,  4),
                                            new WorldPos( 4, 0, -1), new WorldPos( 4, 0,  1), new WorldPos(-3, 0, -3), new WorldPos(-3, 0,  3), new WorldPos( 3, 0, -3),
                                            new WorldPos( 3, 0,  3), new WorldPos(-4, 0, -2), new WorldPos(-4, 0,  2), new WorldPos(-2, 0, -4), new WorldPos(-2, 0,  4),
                                            new WorldPos( 2, 0, -4), new WorldPos( 2, 0,  4), new WorldPos( 4, 0, -2), new WorldPos( 4, 0,  2), new WorldPos(-5, 0,  0),
                                            new WorldPos(-4, 0, -3), new WorldPos(-4, 0,  3), new WorldPos(-3, 0, -4), new WorldPos(-3, 0,  4), new WorldPos( 0, 0, -5),
                                            new WorldPos( 0, 0,  5), new WorldPos( 3, 0, -4), new WorldPos( 3, 0,  4), new WorldPos( 4, 0, -3), new WorldPos( 4, 0,  3),
                                            new WorldPos( 5, 0,  0), new WorldPos(-5, 0, -1), new WorldPos(-5, 0,  1), new WorldPos(-1, 0, -5), new WorldPos(-1, 0,  5),
                                            new WorldPos( 1, 0, -5), new WorldPos( 1, 0,  5), new WorldPos( 5, 0, -1), new WorldPos( 5, 0,  1), new WorldPos(-5, 0, -2),
                                            new WorldPos(-5, 0,  2), new WorldPos(-2, 0, -5), new WorldPos(-2, 0,  5), new WorldPos( 2, 0, -5), new WorldPos( 2, 0,  5),
                                            new WorldPos( 5, 0, -2), new WorldPos( 5, 0,  2), new WorldPos(-4, 0, -4), new WorldPos(-4, 0,  4), new WorldPos( 4, 0, -4),
                                            new WorldPos( 4, 0,  4), new WorldPos(-5, 0, -3), new WorldPos(-5, 0,  3), new WorldPos(-3, 0, -5), new WorldPos(-3, 0,  5),
                                            new WorldPos( 3, 0, -5), new WorldPos( 3, 0,  5), new WorldPos( 5, 0, -3), new WorldPos( 5, 0,  3), new WorldPos(-6, 0,  0),
                                            new WorldPos( 0, 0, -6), new WorldPos( 0, 0,  6), new WorldPos( 6, 0,  0), new WorldPos(-6, 0, -1), new WorldPos(-6, 0,  1),
                                            new WorldPos(-1, 0, -6), new WorldPos(-1, 0,  6), new WorldPos( 1, 0, -6), new WorldPos( 1, 0,  6), new WorldPos( 6, 0, -1),
                                            new WorldPos( 6, 0,  1), new WorldPos(-6, 0, -2), new WorldPos(-6, 0,  2), new WorldPos(-2, 0, -6), new WorldPos(-2, 0,  6),
                                            new WorldPos( 2, 0, -6), new WorldPos( 2, 0,  6), new WorldPos( 6, 0, -2), new WorldPos( 6, 0,  2), new WorldPos(-5, 0, -4),
                                            new WorldPos(-5, 0,  4), new WorldPos(-4, 0, -5), new WorldPos(-4, 0,  5), new WorldPos( 4, 0, -5), new WorldPos( 4, 0,  5),
                                            new WorldPos( 5, 0, -4), new WorldPos( 5, 0,  4), new WorldPos(-6, 0, -3), new WorldPos(-6, 0,  3), new WorldPos(-3, 0, -6),
                                            new WorldPos(-3, 0,  6), new WorldPos( 3, 0, -6), new WorldPos( 3, 0,  6), new WorldPos( 6, 0, -3), new WorldPos( 6, 0,  3),
                                            new WorldPos(-7, 0,  0), new WorldPos( 0, 0, -7), new WorldPos( 0, 0,  7), new WorldPos( 7, 0,  0), new WorldPos(-7, 0, -1),
                                            new WorldPos(-7, 0,  1), new WorldPos(-5, 0, -5), new WorldPos(-5, 0,  5), new WorldPos(-1, 0, -7), new WorldPos(-1, 0,  7),
                                            new WorldPos( 1, 0, -7), new WorldPos( 1, 0,  7), new WorldPos( 5, 0, -5), new WorldPos( 5, 0,  5), new WorldPos( 7, 0, -1),
                                            new WorldPos( 7, 0,  1), new WorldPos(-6, 0, -4), new WorldPos(-6, 0,  4), new WorldPos(-4, 0, -6), new WorldPos(-4, 0,  6),
                                            new WorldPos( 4, 0, -6), new WorldPos( 4, 0,  6), new WorldPos( 6, 0, -4), new WorldPos( 6, 0,  4), new WorldPos(-7, 0, -2),
                                            new WorldPos(-7, 0,  2), new WorldPos(-2, 0, -7), new WorldPos(-2, 0,  7), new WorldPos( 2, 0, -7), new WorldPos( 2, 0,  7),
                                            new WorldPos( 7, 0, -2), new WorldPos( 7, 0,  2), new WorldPos(-7, 0, -3), new WorldPos(-7, 0,  3), new WorldPos(-3, 0, -7),
                                            new WorldPos(-3, 0,  7), new WorldPos( 3, 0, -7), new WorldPos( 3, 0,  7), new WorldPos( 7, 0, -3), new WorldPos( 7, 0,  3),
                                            new WorldPos(-6, 0, -5), new WorldPos(-6, 0,  5), new WorldPos(-5, 0, -6), new WorldPos(-5, 0,  6), new WorldPos( 5, 0, -6),
                                            new WorldPos( 5, 0,  6), new WorldPos( 6, 0, -5), new WorldPos( 6, 0,  5) };

    int renderHeight = 2;

    public int renderDistance;

    /// <summary>
    /// Timer to only unload chunks every 10 update ticks
    /// </summary>
    int timer = 0;

    private void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
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
            for (int i = 0; i < buildList.Count && i < 8; i++)
            {
                Chunk chunk = world.BuildChunk(buildList[0]);
                if(chunk != null)
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
                if (distance > Chunk.chunkSize*10 && !chunk.Value.stayLoaded)
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

    public bool IsInRenderDistance(WorldPos pos) {
        if (Mathf.Abs(pos.x - transform.position.x) <= renderDistance * 16 && Mathf.Abs(pos.y - transform.position.y) <= renderDistance * 16 && Mathf.Abs(pos.z - transform.position.z) <= renderDistance * 16)
            return true;

        return false;
    }

    public List<Vector3> GetRenderDistanceChunks() {
        if (renderDistance < 1) {
            return null;
        }

        int size = renderDistance * 2 + 1;
        List<Vector3> ret = new List<Vector3>(size*size*size);

        for (int z = -renderDistance; z < size; z++) {
            for (int y = -renderDistance; y < size; y++)
            {
                for (int x = -renderDistance; x < size; x++)
                {
                    ret.Add(new Vector3(x, y, z));
                }
            }
        }

        return ret;
    }
}