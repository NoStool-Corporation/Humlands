using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for every input apart from camera movement.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{

    public Player player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (player.destroy)
                    TerrainControl.SetBlock(hit, new AirBlock());
                else
                    TerrainControl.SetBlock(hit, player.getBlockToPlace(), true);
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            player.destroy = !player.destroy;
        }
    }
}