using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtil
{
    public static void Play(string name)
    {
        GameObject.FindWithTag("GlobalAudio").GetComponent<GlobalAudio>().PlayAudio(name);
    }

    public static void Play(string name,float volume)
    {
        GameObject.FindWithTag("GlobalAudio").GetComponent<GlobalAudio>().PlayAudio(name,volume);
    }

    public static void Stop()
    {
        GameObject.FindWithTag("GlobalAudio").GetComponent<GlobalAudio>().StopAudio();
    }
}
