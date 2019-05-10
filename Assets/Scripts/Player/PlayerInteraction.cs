using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Script for every input apart from camera movement.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{

    Player player;
    World world;

    private void Start()
    {
        player = this.GetComponent<Player>();
        world = FindObjectOfType<World>();
    }

    void Update()
    {
        if (Application.isFocused && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) { 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (player.destroy)
                        TerrainControl.SetBlock(hit, new AirBlock());
                    else
                        TerrainControl.SetBlock(hit, player.GetBlockToPlace(), true);
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                player.destroy = !player.destroy;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                player.CycleBlock();
            }

            if (Input.GetKeyDown(KeyCode.U)){
                GameObject g = Instantiate(world.entityPrefab, Camera.main.transform.position, new Quaternion(0, 0, 0, 0));
                Entity e = g.GetComponent<Entity>();
                //world.entities.Add(e);
                WorldPos pp = new WorldPos(30, 20, -5);
                //world.entities[0].GiveRandomName();
            }
        }
    }
}