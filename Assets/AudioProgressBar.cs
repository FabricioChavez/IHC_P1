using UnityEngine;
using UnityEngine.UI; // Required for the Slider component

public class AudioProgressBar : MonoBehaviour
{
    // Assign in the Inspector:
    public AudioSource audioSource;
    public Slider progressBar;

    void Start()
    {
        // Check if a clip is present and playing
        if (audioSource.clip != null)
        {
            // Set the slider's max value to the audio clip length (if using seconds)
            // If you set Max Value to 1 in the inspector, you don't need this line,
            // but the Update logic changes slightly (see below).
            // progressBar.maxValue = audioSource.clip.length; 
        }
    }

    void Update()
    {
        // Only update if the audio source is playing and a clip exists
        if (audioSource.isPlaying && audioSource.clip != null)
        {
            // Calculate the normalized time (0 to 1)
            float normalizedTime = audioSource.time / audioSource.clip.length;

            // Update the slider value
            progressBar.value = normalizedTime;
        }
    }
}