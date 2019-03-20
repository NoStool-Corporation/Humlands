using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DirtBlock : Block
{
    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.DIRT;
    }
}
