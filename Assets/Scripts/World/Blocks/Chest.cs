using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Inventory inventory;


    public Chest() : base()
    {
        inventory = new Inventory(3000);
    }
}