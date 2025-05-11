using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenEvents : MonoBehaviour
{
    [SerializeField]
    GameObject[] toOpen;

    [SerializeField]
    ActiveDialogue dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AmogusDoor()
    {
        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StopMoving();
        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().AnimationChange("IdleUp");

        StartCoroutine(Timer.timer(1,()=>{

            for (int i = 0;i < toOpen.Length;i++)
            {
                toOpen[i].SetActive(true);
                StartCoroutine(Timer.timer(1,()=>{
                    dialog.ActiveDialog();
                
                }));
            }
        }));
    }
}
