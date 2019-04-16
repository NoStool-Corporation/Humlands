using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    Type toPlace;
    public bool destroy;

    private void Start()
    {
        destroy = false;
        toPlace = new StoneBlock().GetType();
    }

    public void CycleBlock()
    {
        if (toPlace == new StoneBlock().GetType())
        {
            toPlace = new WorkTableBlock().GetType();
        } else
        {
            toPlace = new StoneBlock().GetType();
        }
    }

    public Block getBlockToPlace()
    {
        Block instance = (Block)Activator.CreateInstance(toPlace);
        return instance;
    }
}