using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* How to use the camera:
 * 
*/
public class CameraMovement : MonoBehaviour
{
    // General Camera Speed
    public float moveSpeed = 20f;

    public float degToRad = Mathf.PI / 180;

    public float panBorderThickness = 10f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) /*|| Input.mousePosition.y >= Screen.height - panBorderThickness*/)
        {
            Vector3 direction = new Vector3(Mathf.Sin(transform.rotation.eulerAngles.y * degToRad), 0, Mathf.Cos(transform.rotation.eulerAngles.y * degToRad));
            pos += moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.S) /*|| Input.mousePosition.y <= panBorderThickness*/)
        {
            Vector3 direction = new Vector3(Mathf.Sin(transform.rotation.eulerAngles.y * degToRad), 0, Mathf.Cos(transform.rotation.eulerAngles.y * degToRad));
            pos -= moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.A) /*|| Input.mousePosition.x <= panBorderThickness*/)
        {
            Vector3 direction = new Vector3(Mathf.Cos(transform.rotation.eulerAngles.y * degToRad), 0, Mathf.Sin(transform.rotation.eulerAngles.y * degToRad));
            pos -= moveSpeed * Time.deltaTime * direction;
        }
        if (Input.GetKey(KeyCode.D) /*|| Input.mousePosition.x >= Screen.width - panBorderThickness*/)
        {
            Vector3 direction = new Vector3(Mathf.Cos(transform.rotation.eulerAngles.y * degToRad), 0, Mathf.Sin(transform.rotation.eulerAngles.y * degToRad));
            pos += moveSpeed * Time.deltaTime * direction;
        }

        transform.position = pos;
    }
}
