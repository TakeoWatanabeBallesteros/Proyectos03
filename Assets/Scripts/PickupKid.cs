using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKid : MonoBehaviour
{
    public Transform Shoulder;
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
        if (other.tag ==  "Kid")
        {
            other.transform.SetParent(gameObject.transform);
            other.transform.position = Shoulder.position;
            other.transform.rotation = Quaternion.Euler(-90,0,0);
        }
    }
}
