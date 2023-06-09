using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsSelfDestruction : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length -0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
