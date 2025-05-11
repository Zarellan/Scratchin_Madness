using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractDialogue : MonoBehaviour
{
    [SerializeField]
    DialogueVar[] diag;

    bool entered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (entered && Input.GetKeyDown(KeyCode.Z))
            ActiveDialog();
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObject.FindWithTag("PlayerCollision").name == other.name)
            {
                entered = true;
            }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        entered = false;

    }

    void ActiveDialog()
    {
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().SetDialogue(diag);
        GameObject.FindWithTag("Dialogue").GetComponent<Dialogue>().StartDialogue();
    }


}
