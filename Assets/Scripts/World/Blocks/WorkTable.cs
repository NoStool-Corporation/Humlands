using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkTable : Block
{
    public Item craftableItem;

    public void CraftItem(Entity entity)
    {
        craftableItem.Craft(entity);
    }
}
