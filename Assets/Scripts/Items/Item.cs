using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Superclass of all items, do not initialize.
/// </summary>
public class Item
{
    public readonly string name;

    /// <summary>
    /// Do not initialize.
    /// </summary>
    /// <param name="name"></param>
    public Item(string name)
    {
        this.name = name;
        ItemSprites.LoadItemSprite(name);
    }

    public static bool operator ==(Item i1, Item i2)
    {
        return i1.Equals(i2);
    }

    public static bool operator !=(Item i1, Item i2)
    {
        return !i1.Equals(i2);
    }

    public override bool Equals(object obj)
    {
        if (this.name == ((Item)obj).name)
            return true;
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 101;
        hash = hash + name.GetHashCode() * 227;
        return hash;
    }
}
