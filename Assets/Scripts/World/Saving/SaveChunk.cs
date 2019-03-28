using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// Takes all the changed blocks of a chunk and then get's serialized to save the chunk.
/// </summary>
[Serializable]
public class SaveChunk
{
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>()
    /// <summary>
    /// Loops through all blocks of the chunk and saves those marked as changed
    /// </summary>
    /// <param name="chunk"></param>
    public SaveChunk(Chunk chunk)
    {
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            for (int y = 0; y < Chunk.chunkSize; y++)
            {
                for (int z = 0; z < Chunk.chunkSize; z++)
                {
                    if (!chunk.blocks[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(x, y, z);
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
    }
}