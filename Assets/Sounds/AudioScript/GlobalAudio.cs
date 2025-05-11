using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio instance;
    new AudioSource audio;

    [SerializeField]
    AudioClip[] clips;

    [SerializeField]
    bool undestroyable = true;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        if (undestroyable)
        {
            if (instance != null)
                {
                    Destroy(this.gameObject);
                    return;
                }
            DontDestroyOnLoad(this);
            instance = this;
        }
                
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayAudio(string name)
    {
        for (int i = 0;i < clips.Length;i++)
            {
                if (clips[i].name == name)
                {
                    audio.PlayOneShot(clips[i]);
                }
            }
    }

    public void PlayAudio(string name,float volume)
    {
        for (int i = 0;i < clips.Length;i++)
            {
                if (clips[i].name == name)
                {
                    audio.PlayOneShot(clips[i],volume);
                }
            }
    }

    public void StopAudio()
    {
        audio.Stop();
    }



}
