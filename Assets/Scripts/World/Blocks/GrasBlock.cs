using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GrassBlock : Block
{
    public GrassBlock() {
        id = 3;
    }

    public override bool IsSolid(Direction direction)
    {
        return true;
    }

    public override Vector2 TexturePosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:
                return Tilesheet.GRAS_TOP;
            case Direction.DOWN:
                return Tilesheet.DIRT;
        }
        return Tilesheet.GRAS;
    }
}