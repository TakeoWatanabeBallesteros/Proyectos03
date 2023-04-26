using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerv2 : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.LookAt(target);
        }
    }
}
