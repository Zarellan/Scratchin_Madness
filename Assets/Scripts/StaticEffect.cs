using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticEffect : MonoBehaviour
{

    [SerializeField]
    GameObject staticEffect;

    [SerializeField]
    Sprite[] statics; 

    int index;

    public float goombaPosY;

    public float goombaPosX;
    // Start is called before the first frame update
    void Start()
    {
        //DoStatic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoStatic()
    {
        staticEffect.GetComponent<Image>().enabled = true;
    }

    public void DoStaticGoomba()
    {
        staticEffect.GetComponent<Image>().enabled = true;
        GetComponent<Animator>().Play("StaticGoomba");

        GetComponent<AudioSource>().Play();

        GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring("TOO\nLATE");
        GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().ChangeTimeText("TIME\n666");

    }

    public void EndStaticGoomba()
    {
        GetComponent<AudioSource>().Stop();
        staticEffect.GetComponent<Image>().enabled = false;
        GetComponent<Animator>().Play("Static");
        Time.timeScale = 1;
        GameObject.FindWithTag("Mario").GetComponent<Mario>().Revive();
        GameObject.FindWithTag("Mario").transform.position = new Vector2(goombaPosX,goombaPosY+1);
        GameObject.FindWithTag("Mario").GetComponent<Mario>().AnimationChange("jump");
        GameObject.FindWithTag("Mario").GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        SoundtrackUtil.UnPauseMusic();

    
        GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring(0);
        GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().RevealTime();
    }
}
