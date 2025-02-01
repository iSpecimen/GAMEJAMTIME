using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReplay : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isRewinding = false;
    private List<TimeRecord> timeRecords = new List<TimeRecord>();
    public float recordTime = 5f; // How long to store past data'
    private bool isRewindActive = false; // To prevent interruption
    private float rewindTimer = 0f;
    private Vector3 playerStartPosition;

    void Start()
    {
        timeRecords = new List<TimeRecord>();
        rb = GetComponent<Rigidbody2D>();
        playerStartPosition = transform.position; // Starting position of the player
    }

    void Update()
    {
        // Check if R is pressed to trigger the rewind
        if (Input.GetKeyDown(KeyCode.R) && !isRewindActive)
        {
            StartCoroutine(RewindAndReplay());
        }
    }

    private IEnumerator RewindAndReplay()
    {
        isRewindActive = true;
        isRewinding = true;
        rewindTimer = 0f;

        // Store the current index for time records
        int rewindIndex = timeRecords.Count - 1;
        float lastRewindTime = Time.time; // Track the time when we start rewinding
        
        // Start the rewind for 5 seconds
        while (rewindTimer < recordTime && rewindIndex >= 0)
        {
            float deltaTime = Time.time - lastRewindTime; // Calculate delta time for each rewind step
            if (deltaTime >= Time.fixedDeltaTime) // Ensure it's consistent with FixedUpdate
            {
                rewindTimer += deltaTime;
                lastRewindTime = Time.time;

                // Rewind time by applying the last recorded state
                TimeRecord lastRecord = timeRecords[rewindIndex];
                transform.position = lastRecord.position;
                transform.rotation = Quaternion.Euler(0, 0, lastRecord.rotation);
                rb.velocity = -lastRecord.velocity;

                rewindIndex--; // Move backwards through the time records list
            }
            yield return null;
        }

        // After 5 seconds, stop rewind and start replay
        isRewinding = false;

        // Replay the time for the next 5 seconds
        float replayTimer = 0f;
        while (replayTimer < recordTime && rewindIndex == timeRecords.Count - 1)
        {
            replayTimer += Time.deltaTime;
            yield return null;
        }

        // Re-enable regular time
        isRewindActive = false;
    }

    void FixedUpdate()
    {
        // Only record time when not rewinding
        if (!isRewinding)
        {
            timeRecords.Add(new TimeRecord(transform.position, rb.velocity, transform.rotation.eulerAngles.z));
        }
    }

    void RecordTime()
    {
        if (timeRecords.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            timeRecords.RemoveAt(0);
        }
        timeRecords.Add(new TimeRecord(transform.position, rb.velocity, transform.rotation.eulerAngles.z));
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
        return isRewinding;
    }
}
