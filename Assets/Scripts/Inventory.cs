using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory
{
    private List<ItemStack> itemStacks;
    private readonly int maxSize;

    /// <summary>
    /// The InventoryUIManagers displaying this inventory
    /// </summary>
    [NonSerialized]
    private List<InventoryUIManager> uiManagers;

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
        this.uiManagers = new List<InventoryUIManager>();
    }

    /// <summary>
    /// Adds an ItemStack to this inventory, just like the Add(ItemStack) function, but also takes it away from the original ItemStack.
    /// </summary>
    /// <param name="stack"></param>
    /// <returns>Returns the amount of Items transfered as an ItemStack</returns>
    public ItemStack Transfer(ItemStack stack)
    {
        return stack.Remove(this.Add(stack));
    }

    /// <summary>
    /// Adds an ItemStack to this inventory or as much of an ItemStack as there is space left in the inventory.
    /// </summary>
    /// <param name="stack"></param>
    /// <returns>Returns the amount of items added to this inventory as an ItemStack</returns>
    public ItemStack Add(ItemStack stackOriginal)
    {
        ItemStack stack = stackOriginal.Clone() as ItemStack;
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
                UpdateUI();
                return toAdd;
            }
        }
        itemStacks.Add(toAdd);
        UpdateUI();
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

        ItemStack itemsRemoved = removeFrom.Remove(stack);
        UpdateUI();
        return itemsRemoved;
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

    private void UpdateUI()
    {
        foreach (var man in uiManagers)
        {
            man.Render();
        }
    }

    public void AddUIManager(InventoryUIManager man)
    {
        uiManagers.Add(man);
    }

    public void RemoveUIManager(InventoryUIManager man)
    {
        uiManagers.Remove(man);
    }
}