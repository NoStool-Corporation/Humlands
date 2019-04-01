using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemStack> itemStacks;
    private readonly int maxSize;

    /// <summary>
    /// Amount of Items in the Inventory
    /// </summary>
    public int Amount
    {
        get { return GetAmount(); }
    }

    /// <summary>
    /// Amount of ItemStacks in the Inventory
    /// </summary>
    public int StackAmount
    {
        get { return itemStacks.Count;  }
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
    /// Removes Items in the form of an ItemStack from this inventory.
    /// </summary>
    /// <param name="stack">The Stack of Items to remove</param>
    /// <returns>Returns the amount of Items removed as an ItemStack</returns>
    public ItemStack Remove(ItemStack stack)
    {
        ItemStack removeFrom = GetItemStackWithItem(stack.Item);

        if (removeFrom == null)
            return new ItemStack(stack.Item, 0);

        if (removeFrom.Size - stack.Size < 0)
            return removeFrom.Remove(removeFrom.Size);

        return removeFrom.Remove(stack);
    }

    /// <summary>
    /// Returns the amount of items of the specified item type 
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Returns the amount of items of the specified item type</returns>
    public int GetAmountOfItem(Item item)
    {
        ItemStack stack = GetItemStackWithItem(item);
        if (stack != null)
            return stack.Size;
        return 0;
    }

    /// <summary>
    /// Searches for an ItemStack in this inventory which contains the specified item
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Returns the ItemStack with the item, or null if non was found</returns>
    private ItemStack GetItemStackWithItem(Item item)
    {
        for (int i = 0; i < itemStacks.Count; i++)
            if (itemStacks[i].Item == item)
                return itemStacks[i];
        return null;
    }

    /// <summary>
    /// Returns the amount of items in the inventory
    /// </summary>
    /// <returns>Returns the amount of items in the inventory as an int</returns>
    private int GetAmount()
    {
        int amount = 0;
        for(int i = 0; i < itemStacks.Count; i++)
        {
            amount += itemStacks[i].Size;
        }
        return amount;
    }

    /// <summary>
    /// Returns the ItemStack at the specified position.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public ItemStack GetStack(int i)
    {
        return itemStacks[i];
    }
}