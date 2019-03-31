using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Datascructure to save all the information about one entity which are needed to recreate it afer a reload.
/// </summary>
[Serializable]
public class SaveEntity : MonoBehaviour
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

    /// <summary>
    /// Instantiates an GameObject out of the stored data.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns>The Entity Object (Not the GameObject)</returns>
    public Entity Instantiate(GameObject prefab)
    {
        GameObject gameObject = Instantiate(prefab, position, rotation);
        Entity e = gameObject.GetComponent<Entity>();
        e.entityName = entityName;
        e.transform.position = position;
        e.transform.rotation = rotation;
        e.stayLoaded = stayLoaded;
        e.inventory = inventory;
        e.job = job;

        return e;
    }
}
