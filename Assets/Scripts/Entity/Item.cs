using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    static readonly string textureFolder = "Graphics/ItemTextures/";
    readonly string name;

    public Item(string name = "item")
    {
        this.name = name;
    }

    /// <summary>
    /// Returns the Texture2D of the item
    /// </summary>
    /// <returns>Returns the Texture2D of the item</returns>
    public virtual Texture2D GetTexture()
    {
        return Resources.Load<Texture2D>(textureFolder + name);
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
        if (this.GetType() == obj.GetType())
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
