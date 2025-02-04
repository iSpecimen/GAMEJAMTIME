using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRecord
{
    public Vector2 position;
    public Vector2 velocity;
    public float rotation;
    public int animationState; // Stores the animation hash

    public TimeRecord(Vector2 pos, Vector2 vel, float rot, int animState)
    {
        position = pos;
        velocity = vel;
        rotation = rot;
        animationState = animState;
    }
}