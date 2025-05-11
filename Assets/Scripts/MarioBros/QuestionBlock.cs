using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using Unity.VisualScripting;
public class QuestionBlock : MonoBehaviour
{

    bool hitted = false;
    enum Item
    {
        coin,
        mushroom
    }

    [SerializeField]
    GameObject coin,mushroom;


    [SerializeField]
    Item item;


    [SerializeField]
    Sprite emptyBlock;
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
        if (GameObject.FindWithTag("MarioHeadBrick").name == col.collider.name)
            {   
                if (!hitted)
                {
                switch(item)
                {
                    case Item.coin:{
                    AudioUtil.Play("coin",0.6f);
                    GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring(200);
                    GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().AddCoin(1);
                    coin.GetComponent<SpriteRenderer>().enabled = true;
                    Tween.PositionY(coin.transform,coin.transform.position.y + 2.5f,0.3f,Ease.OutSine).OnComplete(()=>{
                    Tween.PositionY(coin.transform,coin.transform.position.y - 2.5f,0.3f,Ease.InSine).OnComplete(()=>{Destroy(coin);});

                    });
                    
                    }break;

                    case Item.mushroom:{


                    mushroom.SetActive(true);
                    StartCoroutine(Timer.timer(0.2f,()=>Tween.PositionY(mushroom.transform,mushroom.transform.position.y+1,1,Ease.Linear).OnComplete(()=>mushroom.GetComponent<Mushroom>().EnableGrow())));

                    };break;
                }

                GetComponent<SpriteRenderer>().sprite = emptyBlock;

                Tween.PositionY(transform,transform.position.y + 0.4f,0.1f,Ease.OutCirc).OnComplete(()=>{
                    Tween.PositionY(transform,transform.position.y - 0.4f,0.1f,Ease.InSine);
                });
                }

                AudioUtil.Play("failedBrick",0.7f);
                hitted = true;
            }

    }

}
