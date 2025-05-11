using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaHit : MonoBehaviour
{
    [SerializeField]
    Goomba goomb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameObject.FindWithTag("Mario").name == col.name && !goomb.stomped)
        {   
            GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring(100);
            goomb.prevDeathY = goomb.transform.position.y;
            goomb.prevDeathX = goomb.transform.position.x;
            goomb.stomped = true;
            AudioUtil.Play("stomp");
            goomb.GetComponent<Animator>().Play("Stomp");
            goomb.GetComponent<Rigidbody2D>().gravityScale = 0;
            goomb.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Timer.timer(0.5f,()=>goomb.gameObject.SetActive(false)));

            float xMario = GameObject.FindWithTag("Mario").GetComponent<Rigidbody2D>().velocity.x;
            GameObject.FindWithTag("Mario").GetComponent<Rigidbody2D>().velocity = new Vector2(xMario,10);
        }

    }

}
