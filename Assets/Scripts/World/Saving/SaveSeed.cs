using System.Collections.Generic;
using UnityEngine;
using System;


// Datascructure to save the seed.

[Serializable]
public class SaveSeed
{
    public int seed = new int();

    public SaveSeed(World worldWithSeed)
    {

        seed = worldWithSeed.seed;
    }
}

