using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CarpenterBlueprintBlock : BlueprintBlock
{
    public override void SetupAfterSerialization()
    {
        base.SetupAfterSerialization();
        goal = typeof(GrassBlock);
        ItemStack required = new ItemStack(new WoodItem(), 4);
        //no product, as the product is the built block in the world
        ItemStack product = new ItemStack(new WoodItem(), 0);
        task = new Task(new ItemStack[] { required }, new ItemStack[] { product }, 100);
    }
}
