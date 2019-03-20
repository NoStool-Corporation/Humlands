using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GrassBlock : Block
{
    public override bool IsSolid(Direction direction)
    {
        return true;
    }

    public override Vector2 TexturePosition(Direction direction)
    {
        switch(direction)
        {
            case Direction.UP:
                return new Vector2(TextureCords.GRAS_TOP, 1);
            case Direction.DOWN:
                return new Vector2(TextureCords.DIRT, 1);
        }
        return new Vector2(TextureCords.GRAS, 1);
    }
}