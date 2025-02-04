using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReplay : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private List<TimeRecord> timeRecords = new List<TimeRecord>();
    public float recordTime = 5f;
    private bool isRewindActive = false;


    public float cooldownTime = 12f; // Cooldown duration
    private float nextUseTime = 0f; // When the ability can be used again

    public GameObject ghostPrefab;
    public GameObject cooldownBar; 



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRewindActive && Time.time >= nextUseTime)
        {
            StartCoroutine(RewindAndReplay());
            nextUseTime = Time.time + cooldownTime; // Set next allowed time
        }
    }

    private IEnumerator RewindAndReplay()
    {
        isRewindActive = true;

        // **Step 1: Spawn Ghost (Only for Player)**
        GameObject ghost = null;
        Rigidbody2D ghostRb = null;
        Animator ghostAnimator = null;
        
        if (gameObject.CompareTag("Player")) // Only spawn ghost if it's the player
        {
            ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            ghostRb = ghost.GetComponent<Rigidbody2D>();
            ghostRb.isKinematic = false; // Allow physics-based interactions

            ghostAnimator = ghost.GetComponent<Animator>();

            SpriteRenderer ghostRenderer = ghost.GetComponent<SpriteRenderer>();
            ghostRenderer.color = new Color(1, 1, 1, 0.5f); // Make ghost semi-transparent
        }

        // **Step 2: Rewind**
        for (int i = timeRecords.Count - 1; i >= 0; i--)
        {
            // transform.position = timeRecords[i].position;
            // transform.rotation = Quaternion.Euler(0, 0, timeRecords[i].rotation);
            // rb.velocity = -timeRecords[i].velocity; // Reverse velocity for smooth rewind

            if (ghost) // Move ghost if it exists
            {
                ghost.transform.position = timeRecords[i].position;
                ghost.transform.rotation = Quaternion.Euler(0, 0, timeRecords[i].rotation);
                ghostRb.velocity = -timeRecords[i].velocity; // Apply ghost velocity

                // Sync animation with the recorded state
                if (ghostAnimator)
                {
                    ghostAnimator.Play(timeRecords[i].animationState);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        // **Step 3: Replay**
        for (int i = 0; i < timeRecords.Count; i++)
        {
            // transform.position = timeRecords[i].position;
            // transform.rotation = Quaternion.Euler(0, 0, timeRecords[i].rotation);
            // rb.velocity = timeRecords[i].velocity; // Apply velocity for accurate replay

            if (ghost) // Move ghost if it exists
            {
                ghost.transform.position = timeRecords[i].position;
                ghost.transform.rotation = Quaternion.Euler(0, 0, timeRecords[i].rotation);
                ghostRb.velocity = timeRecords[i].velocity; // Apply velocity to ghost

                // Sync animation with the recorded state
                if (ghostAnimator)
                {
                    ghostAnimator.Play(timeRecords[i].animationState);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        // **Step 4: Destroy Ghost After Replay**
        if (ghost)
        {
            Destroy(ghost);
        }

        isRewindActive = false;
    }

    void FixedUpdate()
    {
        // Only record time when not rewinding
        if (!isRewindActive)
        {
           RecordTime();
        }
    }

    void RecordTime()
    {
        if (timeRecords.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            timeRecords.RemoveAt(0);
        }
        timeRecords.Add(new TimeRecord(transform.position, rb.velocity, transform.rotation.eulerAngles.z, animator.GetCurrentAnimatorStateInfo(0).fullPathHash));
    }

    // void StartRewind()
    // {
    //     isRewinding = true;
    //     rb.isKinematic = true;
    // }

    // void StopRewind()
    // {
    //     isRewinding = false;
    //     rb.isKinematic = false;
    // }
    
    public bool getRewind()
    {
        
        return isRewindActive;
    }
}
