using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private Vector3 startPos;
    private Vector3 targetPos;

    //Input speed
    private float speed;

    //Speed per axis
    private float aSpeed;

    private WorldPos dir = new WorldPos();
    private long startTime = 0;
    bool[] reached = { false, false, false };
    public bool moving = false;
    public bool paused = false;
    

    /// <summary>
    /// Return the coordinates of the object thats moved according to the movement object
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition() {
        if (paused) 
            return startPos;

        if (!moving) {
            Debug.Log("Target Reached!");
            return targetPos;
        }

        long dt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startTime;

        Vector3 pos = targetPos;
        if (!reached[0])
            pos.x = startPos.x + (dt * aSpeed / 1000 * dir.x);
        if (!reached[1])
            pos.y = startPos.y + (dt * aSpeed / 1000 * dir.y);
        if (!reached[2])
            pos.z = startPos.z + (dt * aSpeed / 1000 * dir.z);

        if ((targetPos.x - pos.x) * dir.x * -1 > 0) {
            reached[0] = true;
            pos.x = targetPos.x;
            ReachedOne(pos);
        }


        if ((targetPos.y - pos.y) * dir.y * -1 > 0) {
            reached[1] = true;
            pos.y = targetPos.y;
            ReachedOne(pos);
        }
            

        if ((targetPos.z - pos.z) * dir.z  * -1 > 0)
        {
            reached[2] = true;
            pos.z = targetPos.z;
            ReachedOne(pos);
        }

        bool all = true;
        for (int i = 0; i < 3; i++)
        {
            if (!reached[i])
            {
                all = false;
                continue;
            }
        }

        if (all)
            moving = false;

        return pos;
    }

    /// <summary>
    /// Called if the target pos of one axis in reached
    /// </summary>
    /// <param name="pos"></param>
    private void ReachedOne(Vector3 pos) {
        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        startPos = pos;
        CalcAxisSpeed();
    }

    /// <summary>
    /// Calculates the speed per axis aka. aSpeed
    /// </summary>
    private void CalcAxisSpeed() {
        aSpeed = 3;
        for (int i = 0; i < 3; i++)
        {
            if (reached[i])
                aSpeed--;
        }

        aSpeed = aSpeed == 0 ? 0 : speed / aSpeed;
    }

    /// <summary>
    /// Updates thed direction Vector3
    /// </summary>
    private void UpdateDir() {
        dir.x = startPos.x <= targetPos.x ? 1 : -1;
        dir.y = startPos.y <= targetPos.y ? 1 : -1;
        dir.z = startPos.z <= targetPos.z ? 1 : -1;
    }

    /// <summary>
    /// Starts the movement -> sets the start time and the start pos, given a the only parameter 
    /// </summary>
    /// <param name="pos"></param>
    public void Start(Vector3 st, float sp, Vector3 ta)
    {
        if (st == null || sp <= 0 || ta == null)
            return;

        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        startPos = st;
        speed = sp;
        targetPos = ta;
        UpdateDir();
        moving = true;

        if ((targetPos.x - startPos.x) * dir.x * -1 > 0)
            reached[0] = true;
        if ((targetPos.y - startPos.y) * dir.y * -1 > 0)
            reached[1] = true;
        if ((targetPos.z - startPos.z) * dir.z * -1 > 0)
            reached[2] = true;

        CalcAxisSpeed();
    }

    /// <summary>
    /// Pauses the movement if it is active / moving == true
    /// </summary>
    /// <returns></returns>
    public bool Pause() {
        if (!moving)
            return false;

        startPos = GetPosition();
        paused = false;
        return true;
    }

    /// <summary>
    /// Resumes the Movement if it is paused
    /// </summary>
    /// <returns></returns>
    public bool Resume() {
        if (!paused)
            return false;

        startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return true;
    }
}
