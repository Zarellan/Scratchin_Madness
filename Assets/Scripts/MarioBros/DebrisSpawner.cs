using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{   
    [SerializeField]
    GameObject debrisFly;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnDepris()
    {
        Instantiate(debrisFly,transform.position,Quaternion.identity);
    }
}
