using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemStack> itemStacks;
    private readonly int maxSize;

    /// <summary>
    /// Amount of items in the Inventory
    /// </summary>
    public int Amount
    {
        get { return getAmount(); }
    }

    /// <summary>
    /// Creates a new inventory of the specified size
    /// </summary>
    /// <param name="maxSize"></param>
    public Inventory(int maxSize)
    {
        this.itemStacks = new List<ItemStack>();
        this.maxSize = maxSize;
    }

    /// <summary>
    /// Adds an ItemStack to this inventory or as much of an ItemStack as there is space left in the inventory.
    /// </summary>
    /// <param name="stack"></param>
    /// <returns>Returns the amount of items added to this inventory as an ItemStack</returns>
    public ItemStack Add(ItemStack stack)
    {
        if (this.Amount == this.maxSize)
            return new ItemStack(stack.Item, 0);

        ItemStack toAdd = stack;

        if (this.Amount + stack.Size > this.maxSize)
            toAdd = new ItemStack(stack.Item, this.maxSize - Amount);

        for (int i = 0; i < itemStacks.Count; i++)
        {
            if (itemStacks[i].Item == toAdd.Item)
            {
                itemStacks[i].Add(toAdd.Size);
                return toAdd;
            }
        }
        itemStacks.Add(toAdd);
        return toAdd;
    }

    /// <summary>
    /// Returns the amount of items of the specified item type 
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Returns the amount of items of the specified item type</returns>
    public int getAmountOfItem(Item item)
    {
        for (int i = 0; i < itemStacks.Count; i++)
            if (itemStacks[i].Item == item)
                return itemStacks[i].Size;
        return 0;
    }

    /// <summary>
    /// Returns the amount of items in the inventory
    /// </summary>
    /// <returns>Returns the amount of items in the inventory as an int</returns>
    private int getAmount()
    {
        int amount = 0;
        for(int i = 0; i < itemStacks.Count; i++)
        {
            amount += itemStacks[i].Size;
        }
        return amount;
    }
}