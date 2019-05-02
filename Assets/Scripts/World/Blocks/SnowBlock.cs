using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SnowBlock : Block
{
    public SnowBlock() {
        id = 5;
    }
    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.SNOW;
    }
}
