using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
    public static int chunkSize = 16;
    public Block[,,] blocks;
    public World world;
    public WorldPos pos;
    MeshFilter filter;
    MeshCollider coll;
    public int updates;

    public bool update = false;
    public bool updateNeighbors = false;
    public bool rendered;

    public Chunk()
    {
        blocks = new Block[chunkSize, chunkSize, chunkSize];
    }

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
        updates = 0;
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    public void SetBlock(int x, int y, int z, Block block, bool update = true)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
            this.update = update;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block, update);
        }
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    private void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
        if (updateNeighbors)
        {
            updateNeighbors = false;
            UpdateNeighbors();
        }

    }

    void UpdateChunk()
    {
        updates++;
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
        rendered = true;
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }

    public void UpdateNeighbors()
    {
        Chunk[] chunks =
        {
            world.GetChunk(pos.x + 16, pos.y, pos.z),
            world.GetChunk(pos.x, pos.y + 16, pos.z),
            world.GetChunk(pos.x, pos.y, pos.z + 16),
            world.GetChunk(pos.x - 16, pos.y, pos.z),
            world.GetChunk(pos.x, pos.y - 16, pos.z),
            world.GetChunk(pos.x, pos.y, pos.z - 16)
        };

        foreach (Chunk c in chunks)
            if (c != null)
                c.update = true;
    }

    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();
        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();
        coll.sharedMesh = mesh;
    }
}
