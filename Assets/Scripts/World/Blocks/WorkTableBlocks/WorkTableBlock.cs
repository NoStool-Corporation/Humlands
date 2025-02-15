﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// The general WorkTableBlock super class, don't create objects from this
/// </summary>
[Serializable]
public class WorkTableBlock : Block
{
    [NonSerialized]
    public Task[] tasks;

    public int currentTask;

    public Inventory inventory;
    [NonSerialized]
    List<InventoryUIManager> uiManagers;

    public WorkTableBlock() : base() {
        inventory = new Inventory(100);
        uiManagers = new List<InventoryUIManager>();
        SetupAfterSerialization();
    }

    /// <summary>
    /// Gets called once the current Task has been completed. Useful for creating the product ItemStack(s)
    /// </summary>
    public virtual void OnTaskComplete(Entity entity)
    {
        for (int i = 0; i < tasks[currentTask].product.Length; i++)
        {
            inventory.Add(tasks[currentTask].product[i]);
        }

        for (int i = 0; i < tasks[currentTask].requiredResources.Length; i++)
        {
            inventory.Remove(tasks[currentTask].requiredResources[i]);
        }
    }

    /// <summary>
    /// Performs a crafting action on the currentTask, reducing the work needed to complete the Task
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>Returns whether the crafting action was successful</returns>
    public bool Craft(Entity entity)
    {
        bool readyToCraft = true;

        //determine if the required items are available
        for (int i = 0; i < tasks[currentTask].requiredResources.Length; i++)
        {
            if(tasks[currentTask].requiredResources[i].Size > inventory.GetAmountOfItem(tasks[currentTask].requiredResources[i].Item))
            {
                //readyToCraft = false;
            }
        }

        if(readyToCraft)
        {
            tasks[currentTask].Craft(this, entity);
            for (int i = 0; i < uiManagers.Count; i++)
            {
                uiManagers[i].Render();
            }
            return true;
        }
        return false;
    }

    public void AddUIManager(InventoryUIManager manager)
    {
        uiManagers.Add(manager);
    }

    public void RemoveUIManager(InventoryUIManager manager)
    {
        uiManagers.Remove(manager);
    }
}
