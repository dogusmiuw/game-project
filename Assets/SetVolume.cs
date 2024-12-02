using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public Slider volumeSlider; // Assign the slider in the inspector
    private AudioSource audioSource;

    void Start()
    {
        // Find the AudioSource in your scene
        audioSource = FindObjectOfType<AudioSource>();

        if (audioSource != null)
        {
            // Initialize slider with current volume
            volumeSlider.value = audioSource.volume;

            // Add listener for slider changes
            volumeSlider.onValueChanged.AddListener(SetAudioVolume);
        }
        else
        {
            Debug.LogError("No AudioSource found in the scene.");
        }
    }

    public void SetAudioVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume; // Update the AudioSource volume
        }
    }
}
