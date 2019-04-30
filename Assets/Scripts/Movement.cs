using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 speed;
    private WorldPos dir;
    private long startTime;
    bool[] reached = { false, false, false };

    /// <summary>
    /// (Re-)Sets the speed
    /// </summary>
    /// <param name="s"></param>
    public void SetSpeed(Vector3 s) {
        speed = s;
    }

    /// <summary>
    /// (Re-)Sets the coordinate of the target point
    /// </summary>
    /// <param name="t"></param>
    public void SetTarget(Vector3 t) {
        targetPos = t;
    }

    /// <summary>
    /// Return the coordinates of the object thats moved according to the movement object
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition() {
        long dt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime;

        Vector3 pos = new Vector3();
        if (!reached[0])
            pos.x = startPos.x + (dt * speed.x * dir.x);
        if (!reached[1])
            pos.y = startPos.y + (dt * speed.y * dir.y);
        if (!reached[2])
            pos.z = startPos.z + (dt * speed.z * dir.z);

        if ((targetPos.x - pos.x) * dir.x > 1)
            reached[0] = true;

        if ((targetPos.y - pos.y) * dir.y > 1)
            reached[1] = true;

        if ((targetPos.z - pos.z) * dir.z > 1)
            reached[2] = true;

        return pos;
    }

    /// <summary>
    /// Updates the Direction Vector3
    /// </summary>
    private void UpdateDir() {
        dir.x = startPos.x < targetPos.x ? 1 : -1;
        dir.y = startPos.y < targetPos.y ? 1 : -1;
        dir.z = startPos.z < targetPos.z ? 1 : -1;
    }

    /// <summary>
    /// Starts the movement -> sets the start time and the start pos, given a the only parameter 
    /// </summary>
    /// <param name="pos"></param>
    public void Start(Vector3 st, Vector3 sp, Vector3 ta) {
        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        startPos = st;
        speed = sp;
        targetPos = ta;
        UpdateDir();
    }
}
