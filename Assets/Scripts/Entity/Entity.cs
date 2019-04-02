using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Entity : MonoBehaviour
{
    public enum Jobs {Miner, Crafter, Baker}


    private string NAME_FILE_PATH = "Assets/Resources/Strings/EntityNames.txt";
    public static string PREFAB_PATH = "Prefabs/Entity";


    public bool stayLoaded = false;
    public Inventory inventory;
    public string entityName;
    public Jobs job;

    private World world;
    

    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<World>();
        world.BuildChunk(new WorldPos((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y), (int)Mathf.Round(transform.position.z))).stayLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Work() {
        stayLoaded = true;
    }

    public void SetPosition(WorldPos pos) {

        if (world.GetChunk(new WorldPos(transform.position)) != world.GetChunk(pos)) {
            world.GetChunk(pos);
            world.BuildChunk(pos);
        }

        transform.position = pos.ToVector3();
    }

    private string[] GetNames() {
        StreamReader reader = new StreamReader(NAME_FILE_PATH);
        string read = reader.ReadToEnd();
        reader.Close();

        string[] ret = read.Split('\n');

        return ret;
    }

    private void GiveRandomName() {
        string[] names = GetNames();
        entityName = names[Random.Range(0, names.Length - 1)];
    }
    
}
