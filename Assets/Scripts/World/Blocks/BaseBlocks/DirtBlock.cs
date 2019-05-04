using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DirtBlock : Block
{
    public DirtBlock() {
        id = 2;
    }

    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.DIRT;
    }
}
