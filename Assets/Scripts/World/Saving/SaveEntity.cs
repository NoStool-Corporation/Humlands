using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Datascructure to save all the information about one entity which are needed to recreate it afer a reload.
/// </summary>
[Serializable]
public class SaveEntity
{
    public string entityName;
    public float[] position = new float[3];
    public float[] rotation = new float[4];
    public Inventory inventory;
    public Entity.Jobs job;

    public SaveEntity(Entity e)
    {

        entityName = e.entityName;
        position[0] = e.transform.position.x;
        position[1] = e.transform.position.y;
        position[2] = e.transform.position.z;

        rotation[0] = e.transform.rotation.x;
        rotation[1] = e.transform.rotation.y;
        rotation[2] = e.transform.rotation.z;
        rotation[3] = e.transform.rotation.w;
        
        inventory = e.inventory;
        job = e.job;
    }
}
