using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Organizes Items into stacks of a stack size and provides functions to access it
/// </summary>
public class ItemStack
{
    /// <summary>
    /// What Item the ItemStack contains
    /// </summary>
    public Item Item { get; }

    private int stackSize;
    /// <summary>
    /// Amount of items in the stack
    /// </summary>
    public int Size
    {
        get { return stackSize; }
    }

    /// <summary>
    /// Creates a new ItemStack
    /// </summary>
    /// <param name="item">Item to be contained in the stack</param>
    /// <param name="stackSize">Amount of items starting in the stack, defaults to 0</param>
    public ItemStack(Item item, int stackSize = 0)
    {
        this.Item = item;
        this.stackSize = stackSize;
    }

    /// <summary>
    /// Adds more items to the stack
    /// </summary>
    /// <param name="amount">Positive amount of items to add</param>
    /// <returns>Returns the amount of items added as an ItemStack</returns>
    public ItemStack Add(int amount)
    {
        if (amount < 0)
            return new ItemStack(this.Item, 0);

        this.stackSize += amount;
        return new ItemStack(this.Item, amount);
    }

    /// <summary>
    /// Combines two ItemStacks of the same type
    /// </summary>
    /// <param name="stack">The ItemStack to add to this ItemStack</param>
    /// <returns>Returns the amount of items added as an ItemStack</returns>
    public ItemStack Add(ItemStack stack)
    {
        if (stack.Item.Equals(this.Item))
        {
            return Add(stack.stackSize); ;
        }
        return new ItemStack(stack.Item, 0);
    }

    /// <summary>
    /// Removes an amount of items from this ItemStack
    /// </summary>
    /// <param name="amount">The positive amount of items to remove</param>
    /// <returns>Returns the amount of items removed</returns>
    public ItemStack Remove(int amount)
    {
        ItemStack returnStack = new ItemStack(this.Item);
        if (this.stackSize >= amount && amount >= 0)
        {
            returnStack.Add(amount);
            this.stackSize -= amount;
        }
        else if (amount >= 0)
        {
            returnStack.Add(this.stackSize);
            this.stackSize = 0;
        }
        return returnStack;
    }
    /// <summary>
    /// Removes an ItemStack of items from this ItemStack
    /// </summary>
    /// <param name="stack">The ItemStack containing the amount of items to remove</param>
    /// <returns>Returns the amount of items removed</returns>
    public ItemStack Remove(ItemStack stack)
    {
        if (stack.Item.Equals(this.Item))
        {
            return Remove(stack.stackSize);
        }
        return new ItemStack(stack.Item, 0);
    }
}
