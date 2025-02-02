using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Script is running on: " + gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);
    }
}