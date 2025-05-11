using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OverlapedObject : MonoBehaviour
{

    public UnityEvent events;

    public UnityEvent eventsOnBack;

    public bool canInteract;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObject.FindWithTag("Player").name == other.name)
            {
                if (canInteract)
                    events.Invoke();
            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
            if (GameObject.FindWithTag("Player").name == other.name)
            {
                if (canInteract)
                    eventsOnBack.Invoke();
            }

    }


    public void Interact() //for event system
    {
        events.Invoke();
    }


    public void Disteract() //for event system
    {
        eventsOnBack.Invoke();
    }

}
