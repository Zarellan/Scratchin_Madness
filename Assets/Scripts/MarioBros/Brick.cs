using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{


    bool bricked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameObject.FindWithTag("MarioHeadBrick").name == col.collider.name && !bricked)
            {   
                if (!GameObject.FindWithTag("Mario").GetComponent<Mario>().IsBig())
                {
                    AudioUtil.Play("failedBrick",0.6f);
                    Tween.PositionY(transform,transform.position.y + 0.4f,0.1f,Ease.OutCirc).OnComplete(()=>{
                        Tween.PositionY(transform,transform.position.y - 0.4f,0.1f,Ease.InSine).OnComplete(()=>bricked = false);
                    });
                }
                else
                {
                    AudioUtil.Play("breakBrick",0.9f); 
                    GameObject.FindWithTag("DebrisSpawner").transform.position = transform.position;
                    GameObject.FindWithTag("DebrisSpawner").GetComponent<DebrisSpawner>().SpawnDepris();
                    GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring(50);
                    Destroy(this.gameObject);
                }


                bricked = true;
            }

    }
}
