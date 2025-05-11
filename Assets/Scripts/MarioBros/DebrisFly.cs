using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisFly : MonoBehaviour
{
    [SerializeField]
    GameObject debris1,debris2,debris3,debris4;

    [SerializeField]
    Sprite[] animation;

    int animationIndex;
    // Start is called before the first frame update
    void Start()
    {
        debris1.GetComponent<Rigidbody2D>().velocity = new Vector2(0,9);
        debris2.GetComponent<Rigidbody2D>().velocity = new Vector2(0,9);
        debris3.GetComponent<Rigidbody2D>().velocity = new Vector2(0,5);
        debris4.GetComponent<Rigidbody2D>().velocity = new Vector2(0,5);

        ChangeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 2;
        debris1.transform.position = new Vector2(debris1.transform.position.x + speed * Time.deltaTime,debris1.transform.position.y);

        debris3.transform.position = new Vector2(debris1.transform.position.x + speed * Time.deltaTime,debris3.transform.position.y);

        debris2.transform.position = new Vector2(debris2.transform.position.x - speed * Time.deltaTime,debris2.transform.position.y);

        debris4.transform.position = new Vector2(debris4.transform.position.x - speed * Time.deltaTime,debris4.transform.position.y);
        
    }

    void ChangeSprite()
    {
        GameObject[] debrises = {debris1,debris2,debris3,debris4};
        for (int i = 0;i < animation.Length;i++)
            {
                debrises[i].GetComponent<SpriteRenderer>().sprite = animation[animationIndex];
            }
        animationIndex++;

        if (animationIndex >= animation.Length)
            animationIndex = 0;
        StartCoroutine(Timer.timer(0.2f,ChangeSprite));
    }
}
