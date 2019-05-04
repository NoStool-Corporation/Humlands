using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SandBlock : Block
{
    public SandBlock() {
        id = 4;
    }

    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.SAND;
    }
}
