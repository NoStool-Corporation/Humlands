﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CarpenterBlock : WorkTableBlock
{
    public CarpenterBlock() : base()
    {

    }
    public override void SetupAfterSerialization()
    {
        base.SetupAfterSerialization();
        ItemStack needed1 = new ItemStack(new WoodItem(), 1);
        ItemStack product1 = new ItemStack(new PlankItem(), 1);
        Task task = new Task(new ItemStack[] { needed1 }, new ItemStack[] { product1 }, 100);
        tasks = new Task[1];
        tasks[0] = task;
        currentTask = 0;
        //inventory = new Inventory(100);
        inventory.Add(new ItemStack(new PlankItem(),2));
        new InventoryUIManager(this);
    }
}