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
    float stoneMountainHeight =20;
    float stoneMountainFrequency = 0.02f;
    float stoneMinHeight = 0;
    float dirtBaseHeight = 4;
    float dirtNoise = 0.02f;
    float dirtNoiseHeight = 1;
    float treeFrequency = 0.4f;
    float treeDensity = 3f;
    int seed;

    float biomeSize = 0.005f;
    int biomeDensityAmount = Mathf.RoundToInt(600*1.4f);
    int grasslandDensity = 200;
    int forestDensity = 400;
    int desertDensity = 500;
    int lakeDensity = 600;
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
        int biome1 = GetNoise(chunk.pos.x + 64000, 0, chunk.pos.z + 64000, biomeSize, biomeDensityAmount);
        int biome2 = GetNoise(chunk.pos.x, 0, chunk.pos.z, biomeSize, biomeDensityAmount);
        int biome = Mathf.FloorToInt(Mathf.Sqrt((biome1 * biome1 + biome2 * biome2))) / 2;
        if (biome <= grasslandDensity || IsInRange(biome, forestDensity, desertDensity) || IsInRange(biome, desertDensity, lakeDensity))
        {
            chunk.biome = "grassland";
        }
        else if (biome <= forestDensity)
        {
            chunk.biome = "forest"; 
        }
        else if (biome <= desertDensity)
        {
            chunk.biome = "desert";
        }
        else if (biome <= lakeDensity)
        {
            chunk.biome = "lake";
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
            int biome1 = GetNoise(x, 0, z, biomeSize, biomeDensityAmount);
            int biome2 = GetNoise(x + 64000, 0, z + 64000, biomeSize, biomeDensityAmount);
            int biomeNumber = Mathf.FloorToInt(Mathf.Sqrt((biome1 * biome1 + biome2 * biome2)) / 2);
            if (y <= stoneHeight)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new StoneBlock(), false);
            } else if (y <= dirtHeight)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new DirtBlock(), false);
            } else if (y == dirtHeight + 1)
            {
                if(IsInRange(biomeNumber, forestDensity, desertDensity))
                {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new SandBlock(), false);
                } else if (IsInRange(biomeNumber, desertDensity, lakeDensity))
                {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new WaterBlock(), false);
                } else {
                    chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new GrassBlock(), false);
                }
            } else if (y == dirtHeight + 2 && IsInRange(biomeNumber, grasslandDensity, forestDensity) && GetNoise(x,0,z, treeFrequency, 100) < treeDensity)
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new TreeBlock(), true);
            } else
            {
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new AirBlock(), false);
            }
        }

        return chunk;
    }

    /// <summary>
    /// Returns whether the input number is between the min (exclusive) and the max (inclusive)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private bool IsInRange(int input, int min, int max)
    {
        if (input > min && input <= max)
        {
            return true;
        }
        return false;
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