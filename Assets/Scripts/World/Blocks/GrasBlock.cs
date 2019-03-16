using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                return new Vector2(1,0);
            case Direction.DOWN:
                return new Vector2(3,0);
            default:
                return new Vector2(2,0);
        }
    }
}