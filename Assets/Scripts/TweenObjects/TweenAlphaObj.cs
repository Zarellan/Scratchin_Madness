using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class TweenAlphaObj : MonoBehaviour
{
    [SerializeField] GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TweenAlpha(string alp)
    {
        string[] alps = alp.Split(',');

        Tween.Alpha(obj.GetComponent<SpriteRenderer>(),float.Parse(alps[0]),float.Parse(alps[1]));

    }
}
