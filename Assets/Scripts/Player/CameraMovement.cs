﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the player camera movement
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 20f;

    public float degToRad = Mathf.PI / 180;

    public float moveBorderThickness = 10f;

    Vector3? pivot;

    bool moveBorder = false;

    void Update()
    {
        if (Application.isFocused && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Movement();
            Rotation();
        }
    }

    /// <summary>
    /// Checks for pressed keys and moves the camera accordingly
    /// </summary>
    void Movement()
    {
        float fwMovement = Input.GetAxis("Vertical");
        float swMovement = Input.GetAxis("Horizontal");

        if (moveBorder)
        {
            if (Input.mousePosition.y >= Screen.height - moveBorderThickness)
                fwMovement++;
            if (Input.mousePosition.y <= moveBorderThickness)
                fwMovement--;
            if (Input.mousePosition.x <= moveBorderThickness)
                swMovement--;
            if (Input.mousePosition.x >= Screen.width - moveBorderThickness)
                swMovement++;
        }

        //basic WASD movement
        Vector3 forward = new Vector3(Round(Mathf.Sin(transform.rotation.eulerAngles.y * degToRad), 3), 0, Round(Mathf.Cos(transform.rotation.eulerAngles.y * degToRad), 3));
        transform.position += forward * moveSpeed * Time.deltaTime * fwMovement;
        Vector3 left = new Vector3(Round(Mathf.Sin((transform.rotation.eulerAngles.y + 90) * degToRad), 3), 0, Round(Mathf.Cos((transform.rotation.eulerAngles.y + 90) * degToRad), 3));
        transform.position += left * moveSpeed * Time.deltaTime * swMovement;


        /*if(Input.GetKey(KeyCode.U))
        {
            ItemStack stack = new ItemStack(new WoodItem(), 10);
            ItemStack stack2 = new ItemStack(new PlankItem(), 22);
            ItemStack stack3 = new ItemStack(new PlankItem(), 5);
            ItemStack stack4 = new ItemStack(new StoneItem(), 15);
            ItemStack stack5 = new ItemStack(new CutStoneItem(), 11);
            Inventory inventory = new Inventory(400);
            inventory.Add(stack);
            inventory.Add(stack2);
            inventory.Add(stack3);
            inventory.Add(stack4);
            inventory.Add(stack5);
            inventory.Add(new ItemStack(new WoodItem(), 66));

            InventoryUIManager UI = new InventoryUIManager(inventory);
        }*/


        //zooming with the scroll wheel
        transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * 20;
        if (transform.position.y < 15)
            transform.position = new Vector3(transform.position.x,15,transform.position.z);
    }

    /// <summary>
    /// Checks for pressed key and rotates and moves the camera accordingly
    /// </summary>
    void Rotation()
    {
        //rotation around y axis
        RaycastHit hitY;
        if (Physics.Raycast(transform.position, transform.forward, out hitY))
        {
            transform.RotateAround(TerrainControl.GetBlockPos(hitY).ToVector3(), new Vector3(0, transform.up.y, 0), Input.GetAxis("CamRotation"));
        }
        //rotation around x & z
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (pivot == null)
            {
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, transform.forward, out hit))
                    return;
                pivot = TerrainControl.GetBlockPos(hit).ToVector3();
            }
            transform.RotateAround((Vector3)pivot, -new Vector3(Round(transform.right.x, 3), 0, Round(transform.right.z, 3)), Input.GetAxis("Mouse Y"));
            if (transform.rotation.eulerAngles.x < 15)
                transform.rotation = Quaternion.Euler(15, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            pivot = null;
        }
    }

    float Round(float f, int precision)
    {
        return Mathf.Round(f * Mathf.Pow(10, precision)) / (float)(Mathf.Pow(10, precision));
    }
}