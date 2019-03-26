using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Utility class, mainly contains functions to work with raycasthits
/// </summary>
public static class TerrainControl
{
    /// <summary>
    /// Converts a Vector3 to a WorldPos
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static WorldPos RoundToBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }
    /// <summary>
    /// Converts a raycasthit to a worldpos
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="adjacent">Whether to return the hit block or the block adjacent to the hit block, defaults to false</param>
    /// <returns> Returns the WorldPos of the hit block</returns>
    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return RoundToBlockPos(pos);
    }
    /// <summary>
    /// Moves the position into the block, so rounding will return the blocks position
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="norm"></param>
    /// <param name="adjacent">Whether to move into the block hit or the block adjacent to the hit block, defaults to false</param>
    /// <returns></returns>
    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return (float)pos;
    }
    /// <summary>
    /// Places the block at the RaycastHit
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="block">The block to place</param>
    /// <param name="adjacent">Whether to replace the hit block or the block adjacent to the hit block, defaults to false</param>
    /// <returns>Returns true if a block was set, otherwise returns false</returns>
    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }
    /// <summary>
    /// Returns the block at the RaycastHit
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="adjacent">Whether to return the hit block or the block adjacent to the hit block, defaults to false</param>
    /// <returns></returns>
    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}