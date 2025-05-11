using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float bpm;
    private AudioSource audioSource;
    [SerializeField] private Intervals[] intervals;

    void Start()
    {
        audioSource = SoundtrackUtil.GetAudio();

    }

    private void Update()
    {
        bpm = SoundtrackUtil.GetBPM();

        if (audioSource.isPlaying)
        {
            foreach(Intervals interval in intervals)
            {
                float sampledTime = audioSource.timeSamples / (audioSource.clip.frequency * interval.GetIntervalLength(bpm));
                interval.CheckForNewInterval(sampledTime);

            }
        }


    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float steps;
    [SerializeField] private UnityEvent trigger;

    [HideInInspector]
    public int lastInterval;

    [SerializeField,Tooltip("odd: plays the beats on odd steps\neven: plays the beats on even steps\nboth or anything else: plays directly")] string oddOrEven;

    public float GetIntervalLength(float bpm)
    {
        return 60.0f / (bpm * steps);
    }

    public void CheckForNewInterval(float interval)
    {
        if (oddOrEven == "odd")
        {
        if (Mathf.FloorToInt(interval) != lastInterval && Mathf.FloorToInt(interval)%2 == 1)
            {
                lastInterval = Mathf.FloorToInt(interval);
                trigger.Invoke();

            }
        }
        else if (oddOrEven == "even")
        {
            if (Mathf.FloorToInt(interval) != lastInterval && Mathf.FloorToInt(interval)%2 == 0)
            {
                lastInterval = Mathf.FloorToInt(interval);
                trigger.Invoke();

            }

        }
        else
        {
            if (Mathf.FloorToInt(interval) != lastInterval)
            {
                lastInterval = Mathf.FloorToInt(interval);
                trigger.Invoke();

            }
        }
    }


}