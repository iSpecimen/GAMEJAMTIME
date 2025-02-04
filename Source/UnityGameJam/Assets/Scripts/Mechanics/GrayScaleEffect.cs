using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrayscaleEffect : MonoBehaviour
{
    private Volume volume;
    private ColorAdjustments colorAdjustments;

    // Reference to the TimeRewind script
    public TimeReplay timeRewindScript;

    void Start()
    {
        // Get the Post-Processing component
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);

        if (timeRewindScript == null)
        {
            Debug.LogError("TimeRewind script reference is not assigned.");
        }
    }

    void Update()
    {
        // Check if the player is rewinding using the isRewinding state
        if (timeRewindScript != null && timeRewindScript.getRewind())
        { 
            StartGrayscale();
        }
        else
        {
            StopGrayscale();
        }
    }

    void StartGrayscale()
    {
        if (colorAdjustments != null)
            Debug.Log("Start GrayScale");  // Add a log to verify its state
            colorAdjustments.saturation.Override(-100f); // Full grayscale
    }

    void StopGrayscale()
    {
        if (colorAdjustments != null)
            colorAdjustments.saturation.Override(0f); // Restore color
    }
}