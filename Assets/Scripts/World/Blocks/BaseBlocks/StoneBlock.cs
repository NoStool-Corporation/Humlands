﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StoneBlock : Block
{

    public override Vector2 TexturePosition(Direction direction)
    {
        return Tilesheet.STONEBLOCK;
    }
}
