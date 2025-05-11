using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    [SerializeField]
    DialogueVar[] diag;
    [SerializeField]
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StopMoving();
        StartCoroutine(Timer.timer(timer,DiagIt));
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }



    void DiagIt()
    {
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().SetDialogue(diag);
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().StartDialogue();
    }
}
