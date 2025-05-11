using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.Android.Types;
using UnityEngine;


public class BPMtest : MonoBehaviour
{
    [SerializeField] float bpm;

    [SerializeField] float beat;

    [SerializeField] GameObject flowerPetal;
    [SerializeField] Sprite[] sprite;

    int beats = 0;


    float timeo;

    bool startBeat = false;
    // Start is called before the first frame update
    void Start()
    {
       //if (GetComponent<AudioSource>().isPlaying)
        //    startBeat = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (startBeat)
        {
        timeo += Time.deltaTime;
        if (60.0f/bpm <= timeo)
        {
            OnBeat();
            timeo = 0;
        }
        }*/



    }
    

    public void OnBeat()
    {

        transform.localScale = new Vector2(transform.localScale.x,1.8f);
        Tween.ScaleY(transform,1.6f,0.3f,Ease.OutCirc);
        
        flowerPetal.GetComponent<SpriteRenderer>().sprite = sprite[0];
        
        StartCoroutine(Timer.timer(0.3f,()=>flowerPetal.GetComponent<SpriteRenderer>().sprite = sprite[1]));
        
    }

    public void OnBeat2()
    {
        transform.localScale = new Vector2(transform.localScale.x,1.5f);
        Tween.ScaleY(transform,1.6f,0.3f,Ease.OutCirc);
    }
}
