using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSprites
{
    static string dir = "Graphics/Items/";
    static Dictionary<string, Sprite> sprites;

    static ItemSprites()
    {
        sprites = new Dictionary<string, Sprite>();
    }

    public static void LoadItemSprite(string name)
    {
        name = name.ToLower();
        MonoBehaviour.print(name);
        if (!sprites.ContainsKey(name))
            sprites.Add(name, Resources.Load<Sprite>(dir + name));
    }

    public static Sprite GetItemSprite(string name)
    {
        name = name.ToLower();
        sprites.TryGetValue(name, out Sprite tmp);
        return tmp;
    }
}
