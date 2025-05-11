using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMtest2 : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource in the Inspector
    public float bpm = 130f; // Known BPM of the track
    private float beatInterval; // Time between beats in seconds
    private float nextBeatTime; // When the next beat is expected
    private float[] spectrumData = new float[256]; // FFT data

    void Start()
    {
        // Calculate the interval between beats based on the BPM
        beatInterval = 60f / bpm;
        nextBeatTime = Time.time + beatInterval;
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            DetectBeats();
        }
    }

    void DetectBeats()
    {
        // Get the audio spectrum data
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Analyze low frequencies (e.g., bass)
        float sum = 0f;
        for (int i = 0; i < 10; i++) // Focus on low frequencies
        {
            sum += spectrumData[i];
        }

        // Check if it's time for the next beat
        if (Time.time >= nextBeatTime && sum > 0.1f) // Adjust 0.1f as a base noise threshold
        {
            OnBeatDetected();
            nextBeatTime = Time.time + beatInterval; // Set next expected beat
        }
    }

    void OnBeatDetected()
    {
        Debug.Log("Beat detected!");
        // Add beat reaction logic here (e.g., visuals, events)
    }
}


