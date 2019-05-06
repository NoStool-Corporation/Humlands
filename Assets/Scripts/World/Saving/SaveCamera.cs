using System.Collections.Generic;
using UnityEngine;
using System;


// Datascructure to save all the information about the camera  which are needed to recreate it afer a reload
// (position and rotation).

[Serializable]
public class SaveCamera
{
    public float[] position = new float[3];
    public float[] rotation = new float[4];

    public SaveCamera(Camera cam)
    {

        position[0] = cam.transform.position.x;
        position[1] = cam.transform.position.y;
        position[2] = cam.transform.position.z;

        rotation[0] = cam.transform.rotation.x;
        rotation[1] = cam.transform.rotation.y;
        rotation[2] = cam.transform.rotation.z;
        rotation[3] = cam.transform.rotation.w;
    }
}

