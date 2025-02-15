﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TreeBlock : Block
{

    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.DIRT;
    }
    public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        BlockDataCollisionOnly(chunk, x, y, z, meshData);
        GameObject.Destroy(customModel);
        customModel = GameObject.Instantiate(LoadModels.TreeModel,new Vector3(chunk.pos.x  + x,chunk.pos.y + y - 0.5f,chunk.pos.z + z),Quaternion.Euler(70, 20, 0));
        customModel.transform.Rotate(new Vector3(20, -10, 0));
        return meshData;
    }

    public override bool IsSolid(Direction direction)
    {
        return false;
    }
}