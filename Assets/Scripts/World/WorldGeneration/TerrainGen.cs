using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
/// <summary>
/// Uses noise based on the world seed to generate the blocks into chunks
/// </summary>
public class TerrainGen
{

    float stoneBaseHeight = 0;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 1;
    float stoneMountainHeight =3;
    float stoneMountainFrequency = 0.02f;
    float stoneMinHeight = 0;
    float dirtBaseHeight = 4;
    float dirtNoise = 0.02f;
    float dirtNoiseHeight = 1;
    float forestFrequency = 0.01f;
    float treeFrequency = 0.2f;
    float treeDensity = 10f;
    int seed;

    public TerrainGen(int seed)
    {
        this.seed = seed;
    }

    /// <summary>
    /// Generates a chunk.
    /// </summary>
    /// <param name="chunk">Chunk which needs to be generated</param>
    /// <returns>Generated chunk</returns>
    public Chunk ChunkGen(Chunk chunk)
    {
        //set the biome of the chunk
        int Biome = GetNoise(chunk.pos.x, 0, chunk.pos.z, forestFrequency, 2);
        if(Biome == 1)
        {
            chunk.biome = "grassland";
        }
        else
        {
            chunk.biome = "forest"; 
        }
        
        //generate each column of the chunk
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++)
        {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    /// <summary>
    /// Generates a single column of a chunk
    /// </summary>
    /// <param name="chunk">Chunk in which to generate the column of blocks</param>
    /// <param name="x">Local x coordinate of the column</param>
    /// <param name="z">Local y coordinate of the column</param>
    /// <param name="seed">World seed</param>
    /// <returns>The chunk with the added generated column of blocks</returns>
    private Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        //set the seed of the noise to the world seed
        Noise.Seed = seed;

        //calculate stone height in this column
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        //calculate dirt height in this column
        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        //go through every block in the column and place blocks according to the calculated values
        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            if (y <= stoneHeight)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new StoneBlock(), false);
            } else if (y <= dirtHeight)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new DirtBlock(), false);
            } else if (y == dirtHeight+1)
            {
                chunk.SetBlock(x-chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new GrassBlock(), false);
            } else if (y == dirtHeight + 2 && GetNoise(x,y,z, forestFrequency, 2) == 1 && GetNoise(x,0,z, treeFrequency, 100) < treeDensity)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new TreeBlock(), false);
            } else
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new AirBlock(), false);
            }
        }

        return chunk;
    }
    /// <summary>
    /// Simpler way to generate an int using 3D noise
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="scale">higher scale means a smoother transition when changing x, y and z</param>
    /// <param name="max">maximum return value</param>
    /// <returns>Returns an int between (including) zero and (excluding) max </returns>
    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}