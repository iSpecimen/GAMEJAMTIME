using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecord
{
    public Vector2 position;
    public Vector2 velocity;
    public float rotation;

    public TimeRecord(Vector2 pos, Vector2 vel, float rot)
    {
        position = pos;
        velocity = vel;
        rotation = rot;
    }
}