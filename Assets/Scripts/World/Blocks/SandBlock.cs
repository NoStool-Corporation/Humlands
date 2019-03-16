using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBlock : Block
{
    public override Vector2 TexturePosition(Direction direction)
    {
        return new Vector2(0,1);
    }
}
