using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    string scene;

    [SerializeField]
    float duration;

    [SerializeField]
    float startX,startY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene()
    {
        GameObject.FindWithTag("Transition").GetComponent<Transition>().TransScene(scene,duration);
    }
}
