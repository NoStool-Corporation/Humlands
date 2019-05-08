using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Chest : Block
{
    public Inventory inventory;


    public Chest() : base()
    {
        inventory = new Inventory(5000);
    }
}