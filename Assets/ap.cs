using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Fire")) Debug.Log("Si Hombre");
    }

    private void OnTriggerExit(Collider other)
    { 
        if(other.CompareTag("Fire")) Debug.Log("Si Hombre");
    }
}
