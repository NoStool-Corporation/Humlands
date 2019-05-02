using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StoneBlock : Block
{
    public StoneBlock() {
        id = 1;
    }

    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.STONEBLOCK;
    }
}
