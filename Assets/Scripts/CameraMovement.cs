using System.Collections;
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


    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - moveBorderThickness)
        {
            Vector3 direction = new Vector3(Round(Mathf.Sin(transform.rotation.eulerAngles.y * degToRad),3), 0, Round(Mathf.Cos(transform.rotation.eulerAngles.y * degToRad),3));
            pos += moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= moveBorderThickness)
        {
            Vector3 direction = new Vector3(Round(Mathf.Sin(transform.rotation.eulerAngles.y * degToRad),3), 0, Round(Mathf.Cos(transform.rotation.eulerAngles.y * degToRad),3));
            pos -= moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= moveBorderThickness)
        {
            Vector3 direction = new Vector3(Round(Mathf.Sin((transform.rotation.eulerAngles.y + 90) * degToRad),3), 0, Round(Mathf.Cos((transform.rotation.eulerAngles.y+90) * degToRad),3));
            pos -= moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - moveBorderThickness)
        {
            Vector3 direction = new Vector3(Round(Mathf.Sin((transform.rotation.eulerAngles.y + 90) * degToRad),3), 0, Round(Mathf.Cos((transform.rotation.eulerAngles.y + 90) * degToRad),3));
            pos += moveSpeed * Time.deltaTime * direction;
        }

        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - moveBorderThickness)
        {
            Vector3 direction = new Vector3(Round(Mathf.Sin((transform.rotation.eulerAngles.y + 90) * degToRad), 3), 0, Round(Mathf.Cos((transform.rotation.eulerAngles.y + 90) * degToRad), 3));
            pos += moveSpeed * Time.deltaTime * direction;
        }

        //temporary stuff for presentation
        pos.y += Input.GetAxis("Mouse ScrollWheel")*5;

        if (Input.GetKey(KeyCode.X))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                TerrainControl.SetBlock(hit, new AirBlock());
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                TerrainControl.SetBlock(hit, new StoneBlock(), true);
            }
        }

        transform.position = pos;
    }

    float Round(float f, int precision)
    {
        return Mathf.Round(f * Mathf.Pow(10, precision)) / (float)(Mathf.Pow(10, precision));
    }
}
