using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A chunk is used to render and manage a 16x16x16 area of blocks in the world
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    public string biome;
    public static int chunkSize = 16;
    public Block[,,] blocks;
    private List<Block> updateBlocks;
    public World world;
    public WorldPos pos = new WorldPos();
    private WorldPos lastp = new WorldPos();
    MeshFilter filter;
    MeshCollider coll;

    public bool render = false;
    public bool renderNeighbors = false;
    public bool rendered;
    public bool stayLoaded = false;

    public Chunk()
    {
        blocks = new Block[chunkSize, chunkSize, chunkSize];
        updateBlocks = new List<Block>();
        
    }

    void Start()
    {
        lastp = pos;
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();

    }

    /// <summary>
    /// Deletes all data like custom models of the blocks to prevent memory leak
    /// </summary>
    private void OnDestroy()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if(blocks[x, y, z] != null)
                        blocks[x, y, z].DeleteData();
                }
            }
        }

    }

    /// <summary>
    /// Takes local coordinates to return the block
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>Returns the block at the specified local coordinate</returns>
    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);

    }
    /// <summary>
    /// Places a block at the specified local coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="block"></param>
    /// <param name="render">Whether to rerender the whole chunk after placing the block</param>
    public void SetBlock(int x, int y, int z, Block block, bool render = true)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            if (blocks[x, y, z] != null)
            {
                RemoveUpdateBlock(blocks[x, y, z]);
                blocks[x, y, z].DeleteData();
            }
            blocks[x, y, z] = block;
            block.OnPlacement(this);
            this.render = render;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block, render);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Returns true if index is between 1 and 15 (inclusive)</returns>
    private static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    /// <summary>
    /// Adds a block to the list of update blocks
    /// </summary>
    public void AddUpdateBlock(Block block)
    {
        updateBlocks.Add(block);
    }

    /// <summary>
    /// Removes a block from the list of update blocks
    /// </summary>
    /// <param name="block"></param>
    public void RemoveUpdateBlock(Block block)
    {
        updateBlocks.Remove(block);
    }

    /// <summary>
    /// Updates all the blocks that need to get updated
    /// </summary>
    private void UpdateBlocks()
    {
        foreach (var block in updateBlocks)
        {
            block.Update();
        }
    }

    private void Update()
    {
        if (lastp != pos) {
            Debug.Log(lastp.x.ToString() + " " + pos.x);
            lastp = pos;
        }
        UpdateBlocks();
        if (render)
        {
            render = false;
            RenderChunk();
        }
        if (renderNeighbors)
        {
            renderNeighbors = false;
            RenderNeighbors();
        }

    }
    /// <summary>
    /// Renders the whole chunk
    /// </summary>
    void RenderChunk()
    {
        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
        MakeCollisionMesh(meshData);
        rendered = true;
    }
    /// <summary>
    /// Sets every block inside this chunk to unmodified
    /// </summary>
    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }
    /// <summary>
    /// Queues a rerender for every already rendered neighbor chunk. &lt;br&gt;
    /// Useful when generating new chunks and recalculating which sides can be seen by the player
    /// </summary>
    public void RenderNeighbors()
    {
        Chunk[] chunks =
        {
            world.GetChunk(pos.x + chunkSize, pos.y, pos.z),
            world.GetChunk(pos.x, pos.y + chunkSize, pos.z),
            world.GetChunk(pos.x, pos.y, pos.z + chunkSize),
            world.GetChunk(pos.x - chunkSize, pos.y, pos.z),
            world.GetChunk(pos.x, pos.y - chunkSize, pos.z),
            world.GetChunk(pos.x, pos.y, pos.z - chunkSize)
        };

        foreach (Chunk c in chunks)
            if (c != null && c.rendered)
                c.render = true;
    }
    /// <summary>
    /// Renders the meshData of the chunk
    /// </summary>
    /// <param name="meshData"></param>
    void RenderMesh(MeshData meshData)
    {
        if (filter.sharedMesh != null)
            Destroy(filter.sharedMesh);
        Mesh mesh = new Mesh
        {
            vertices = meshData.vertices.ToArray(),
            triangles = meshData.triangles.ToArray(),
            uv = meshData.uv.ToArray()
        };
        mesh.RecalculateNormals();
        filter.mesh = mesh;
    }

    /// <summary>
    /// Creates the collider Mesh based on the meshData
    /// </summary>
    /// <param name="meshData"></param>
    void MakeCollisionMesh(MeshData meshData)
    {
        if (coll.sharedMesh != null)
            coll.sharedMesh.Clear();

        Mesh mesh = new Mesh
        {
            vertices = meshData.colVertices.ToArray(),
            triangles = meshData.colTriangles.ToArray()
        };
        mesh.RecalculateNormals();
        coll.sharedMesh = mesh;
    }

    public override bool Equals(object o)
    {
        if (o.GetType() != this.GetType())
            return false;
        WorldPos otherPos = ((Chunk)o).pos;
        if (pos.x == otherPos.x && pos.y == otherPos.y && pos.z == otherPos.z)
            return true;
        return false;
    }

    public override int GetHashCode()
    {
        int hash = base.GetHashCode();
        hash = (hash + pos.x) * 1327;
        hash = (hash + pos.y) * 2591;
        hash = (hash + pos.z) * 521;
        return hash;
    }
    
    public WorldPos[] GetBlocksOfType(Type t)
    {
        List<WorldPos> cords = new List<WorldPos>();
        Block block;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    block = GetBlock(x,y,z);
                    if (block.GetType() ==  t) {
                        cords.Add(new WorldPos(pos.x + x, pos.y + y, pos.z + z));
                    }
                        
                }
            }
        }

        return cords.Count > 1 ? cords.ToArray() : null;
    }
}
