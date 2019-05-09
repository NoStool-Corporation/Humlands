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

    /// <summary>
    /// Cycles through the blocks the player can place
    /// </summary>
    public void CycleBlock()
    {
        if (toPlace == typeof(StoneBlock))
        {
            toPlace = typeof(CarpenterBlock);
        } else
        {
            toPlace = typeof(StoneBlock);
        }   
    }

    /// <summary>
    /// Returns a new instance of the block to place currently selected by the player
    /// </summary>
    /// <returns></returns>
    public Block GetBlockToPlace()
    {
        Block instance = (Block)Activator.CreateInstance(toPlace);
        return instance;
    }
}