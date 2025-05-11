using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Goomba : MonoBehaviour
{

    public bool stomped = false;



    [SerializeField]
    UnityEvent eventWhenDied;


    public float prevDeathY,prevDeathX = 0;

    int direction = 1;

    public int Direction
    {
        get{return direction;}
        set {direction = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stomped)
        {
            transform.position = new Vector2(transform.position.x - 2 * direction *Time.deltaTime,transform.position.y);
        }
        else
        {
            transform.position = new Vector2(prevDeathX,prevDeathY);
        }

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameObject.FindWithTag("Mario").name == col.collider.name && !stomped)
            {   
                GameObject.FindWithTag("Mario").GetComponent<Mario>().Damage();
                GameObject.FindWithTag("StaticEffect").GetComponent<StaticEffect>().goombaPosY = transform.position.y;
                GameObject.FindWithTag("StaticEffect").GetComponent<StaticEffect>().goombaPosX = transform.position.x;
                if (!GameObject.FindWithTag("Mario").GetComponent<Mario>().IsBig())
                    eventWhenDied.Invoke();
            }


    }


    public void Stomped()
    {
        stomped = true;
    }



}
