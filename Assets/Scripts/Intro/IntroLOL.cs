using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class IntroLOL : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!GetComponent<VideoPlayer>().isPlaying && GetComponent<VideoPlayer>().time > 1)
        {
            SceneManager.LoadScene("MelodiiRoom");
            GameObject.FindWithTag("Transition").GetComponent<Transition>().ForceTransOut(0.2f);
        }

    }
}
