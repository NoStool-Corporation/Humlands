using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction { NORTH, EAST, SOUTH, WEST, UP, DOWN };

[Serializable]
public class Block
{

    public const float tileSize = 0.1f;

    public virtual Vector2 TexturePosition(Direction direction)
    {
        Vector2 tile = new Vector2();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Vector2 tilePos = TexturePosition(direction);
        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);
        return UVs;
    }

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

}
