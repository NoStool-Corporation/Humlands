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
                return new Vector2(1,4);
            case Direction.DOWN:
                return new Vector2(7,4);
        }
        return new Vector2(4, 4);
    }
}