using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Jobs {Miner, Crafter, Baker}

    private Vector3 position;
    public bool stayLoaded = false;
    private Inventory inventory;
    private Vector3 target;
    private bool kriminalUnderwaygs = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void Save() {

    }

    public Vector3 GetPosition() { return position; }

    public void SetPosition(Vector3 pos) { position = pos; }

    public virtual void move() { }
}
