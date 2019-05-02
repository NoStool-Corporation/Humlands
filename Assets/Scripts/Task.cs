using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Task
{
    public ItemStack[] requiredResources { get; }
    public ItemStack[] product { get; }
    public int workToComplete;
    public int workNeeded;


    /// <summary>
    /// Creates a new Task with the specified requirements and prodcuts.
    /// </summary>
    /// <param name="neededResources"></param>
    /// <param name="product"></param>
    /// <param name="workToComplete">Amount of ticks the task requires to complete, with 20 ticks = 1 second</param>
    public Task(ItemStack[] neededResources, ItemStack[] product, int workToComplete)
    {
        this.product = product;
        this.workToComplete = workToComplete;
        this.requiredResources= neededResources;
        this.workNeeded = workToComplete;
    }

    /// <summary>
    /// Reduces the work left to complete the task and calls the OnTaskComplete of the workTable if the task is done
    /// </summary>
    /// <param name="workTable"></param>
    /// <param name="entity">entity that is crafting at the workTable</param>
    public void Craft(WorkTableBlock workTable, Entity entity)
    {
        workNeeded--;
        if(workNeeded <= 0)
        {
            workTable.OnTaskComplete(entity);
            workNeeded = workToComplete;
        }
    }
}
