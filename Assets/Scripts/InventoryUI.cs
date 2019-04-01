using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Canvas))]
public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public InventoryUI(Inventory inventory)
    {
        this.inventory = inventory;
    }



    public void Render()
    {

    }

}
