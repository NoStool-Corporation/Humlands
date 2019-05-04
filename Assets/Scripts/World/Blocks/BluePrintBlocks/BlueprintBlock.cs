using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlueprintBlock : Block
{
    [NonSerialized]
    public Type goal;
    [NonSerialized]
    public Task task;
    public Inventory inventory;

    public BlueprintBlock()
    {
        inventory = new Inventory(100);
    }

    /// <summary>
    /// Replaces the blueprint with the real block after the task got completed
    /// </summary>
    /// <param name="world"></param>
    /// <param name="pos"></param>
    public virtual void OnTaskComplete(World world, WorldPos pos)
    {
        world.SetBlock(pos, (Block)Activator.CreateInstance(goal));
    }

    /// <summary>
    /// Checks if there are enough items to build the block and then works on the task until completion
    /// </summary>
    /// <param name="world"></param>
    /// <param name="pos">The position of this blueprint in world coordinates</param>
    public void WorkOnToBuild(World world, WorldPos pos)
    {
        bool readyToBuild = true;
        for (int i = 0; i < task.requiredResources.Length; i++)
        {
            if (inventory.GetAmountOfItem(task.requiredResources[i].Item) < task.requiredResources[i].Size)
            {
                readyToBuild = false;
            }
        }

        if (readyToBuild)
            task.Craft(this, world, pos);
    }
}
