using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction { NORTH, EAST, SOUTH, WEST, UP, DOWN };
/// <summary>
/// Super class for all blocks, don't use create objects from this
/// </summary>
[Serializable]
public class Block
{
    public bool changed = true;

    [NonSerialized()]
    public GameObject customModel;
    
    /// <summary>
    /// Gets called by the chunk once the block is actually placed into a chunk and before the chunk gets rerendered.
    /// You can use this method to add this block to the updateBlocks list of the chunk using chunk.AddUpdateBlock(this)
    /// </summary>
    /// <param name="chunk"></param>
    public virtual void OnPlacement(Chunk chunk)
    {

    }

    /// <summary>
    /// If this block is marked as a block that needs updates in it's chunk, this method gets called every frame.
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// Deletes additional data like a custom model to prevent memory leaks
    /// </summary>
    public virtual void DeleteData()
    {
        GameObject.Destroy(customModel);
    }

    /// <summary>
    /// Returns the position of the block's texture in the tilesheet based on the specified direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>Returns a Vector2 with the position of the texture</returns>
    public virtual Vector2 TexturePosition(Direction direction)
    {
        Vector2 tile = new Vector2
        {
            x = 0,
            y = 0
        };
        return tile;
    }
    /// <summary>
    /// Creates all the UVs for a side of the block
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>Returns an array with the UVs</returns>
    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Vector2 tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(Tilesheet.tileSize.x * tilePos.x + Tilesheet.tileSize.x,
            Tilesheet.tileSize.y * tilePos.y);
        UVs[1] = new Vector2(Tilesheet.tileSize.x * tilePos.x + Tilesheet.tileSize.x,
            Tilesheet.tileSize.y * tilePos.y + Tilesheet.tileSize.y);
        UVs[2] = new Vector2(Tilesheet.tileSize.x * tilePos.x,
            Tilesheet.tileSize.y * tilePos.y + Tilesheet.tileSize.y);
        UVs[3] = new Vector2(Tilesheet.tileSize.x * tilePos.x,
            Tilesheet.tileSize.y * tilePos.y);
        return UVs;
    }
    /// <summary>
    /// Creates the rendering and collision data based on surrounding blocks
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData">The MeshData into which the addditional mesh data gets put</param>
    /// <returns>Returns the MeshData containing this block</returns>
    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;
        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
            meshData = FaceDataUp(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.UP))
            meshData = FaceDataDown(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.SOUTH))
            meshData = FaceDataNorth(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.NORTH))
            meshData = FaceDataSouth(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.WEST))
            meshData = FaceDataEast(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.EAST))
            meshData = FaceDataWest(chunk, x, y, z, meshData);

        return meshData;
    }
    /// <summary>
    /// Checks whether the block is solid towards the specified direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>Returns true if the block is solid towards the specified side, otherwise returns false</returns>
    public virtual bool IsSolid(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                return true;
            case Direction.EAST:
                return true;
            case Direction.SOUTH:
                return true;
            case Direction.WEST:
                return true;
            case Direction.UP:
                return true;
            case Direction.DOWN:
                return true;
        }
        return false;
    }
    /// <summary>
    /// Adds the MeshData for the top side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.UP));
        return meshData;
    }
    /// <summary>
    /// Adds the MeshData for the bottom side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.DOWN));
        return meshData;
    }
    /// <summary>
    /// Adds the MeshData for the northern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.NORTH));
        return meshData;
    }
    /// <summary>
    /// Adds the MeshData for the eastern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.EAST));
        return meshData;
    }
    /// <summary>
    /// Adds the MeshData for the southern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.SOUTH));
        return meshData;
    }
    /// <summary>
    /// Adds the MeshData for the western side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.WEST));
        return meshData;
    }

    /// <summary>
    /// Adds the CollisionMeshData for the top side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataUpCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }
    /// <summary>
    /// Adds the CollisionMeshData for the bottom side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataDownCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }
    /// <summary>
    /// Adds the CollisionMeshData for the northern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataNorthCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }
    /// <summary>
    /// Adds the CollisionMeshData for the eastern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataEastCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }
    /// <summary>
    /// Adds the CollisionMeshData for the southern side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataSouthCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }
    /// <summary>
    /// Adds the CollisionMeshData for the western side of the block to the MeshData
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="meshData"></param>
    /// <returns>Returns the updated MeshData</returns>
    protected virtual MeshData FaceDataWestCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertexCollisionOnly(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTrianglesCollisionOnly();
        return meshData;
    }

    public virtual MeshData BlockDataCollisionOnly(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
            meshData = FaceDataUpCollisionOnly(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.UP))
            meshData = FaceDataDownCollisionOnly(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.SOUTH))
            meshData = FaceDataNorthCollisionOnly(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.NORTH))
            meshData = FaceDataSouthCollisionOnly(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.WEST))
            meshData = FaceDataEastCollisionOnly(chunk, x, y, z, meshData);

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.EAST))
            meshData = FaceDataWestCollisionOnly(chunk, x, y, z, meshData);

        return meshData;
    }
}
