using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Transition : MonoBehaviour
{
    public static Transition instance;

    [SerializeField]
    GameObject black;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }


    public void TransScene(string scene,float duration)
    {
        Tween.Alpha(black.GetComponent<Image>(),1,duration);
        StartCoroutine(Timer.timer(duration + 0.1f,()=>{
            SceneManager.LoadScene(scene);
            Tween.Alpha(black.GetComponent<Image>(),0,duration);
        }));
    }

    public void ForceTransOut(float duration)
    {
        black.GetComponent<Image>().color = new Color(0,0,0,1);
        Tween.Alpha(black.GetComponent<Image>(),0,duration);
    }
}
