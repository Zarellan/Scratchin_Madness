using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{

    
    bool canGrow;

    string direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = Random.Range(0,2) == 0?"right":"left";

        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGrow)
            {
                if (direction == "right")
                    transform.position = new Vector2(transform.position.x + 3 * Time.deltaTime,transform.position.y);
                else if (direction == "left")
                    transform.position = new Vector2(transform.position.x - 3 * Time.deltaTime,transform.position.y);
            }
    }

    public void EnableGrow()
    {
        GetComponent<BoxCollider2D>().enabled= true;
        GetComponent<Rigidbody2D>().gravityScale = 4;

        canGrow = true;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameObject.FindWithTag("Mario").name == col.collider.name && canGrow)
            {   
                GameObject.FindWithTag("Mario").GetComponent<Mario>().GrowUp();
                GameObject.FindWithTag("MarioScoreSystem").GetComponent<ScoreSystem>().Scoring(1000);
                Destroy(this.gameObject);
            }

    }

}
