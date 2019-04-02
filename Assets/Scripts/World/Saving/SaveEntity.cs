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
    public Vector3 position;
    public Quaternion rotation;
    public bool stayLoaded = false;
    public Inventory inventory;
    public Entity.Jobs job;

    public SaveEntity(Entity e)
    {
        entityName = e.entityName;
        position = e.transform.position;
        rotation = e.transform.rotation;
        stayLoaded = e.stayLoaded;
        inventory = e.inventory;
        job = e.job;
    }
}
