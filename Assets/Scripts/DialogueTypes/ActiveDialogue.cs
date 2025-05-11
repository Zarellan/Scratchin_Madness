using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDialogue : MonoBehaviour
{
    [SerializeField]
    DialogueVar[] diag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ActiveDialog()
    {
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().SetDialogue(diag);
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().StartDialogue();
    }
}
