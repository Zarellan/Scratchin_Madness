using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour
{
    
    public UnityEvent events;

    public UnityEvent eventsOnBack;

    bool evented = false;

    bool inside;

    bool pressed = false;

    [SerializeField]
    bool hasReverse = false;

    [SerializeField]
    bool repeatable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inside && Input.GetKeyDown(KeyCode.Z) && !pressed)
        {
            
            ActiveEvent(evented);
            CheckIfPressed();
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObject.FindWithTag("Player").name == other.name)
            {
                inside = true;
            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
            if (GameObject.FindWithTag("Player").name == other.name)
            {
                inside = false;
            }

    }

    void CheckIfPressed()
    {
        if (!repeatable)
            pressed = true;

        if (hasReverse)
            evented = evented?false:true;
    }


    void ActiveEvent(bool evented)
    {
        if (!evented)
            events.Invoke();
        else
            eventsOnBack.Invoke();
    }


}
