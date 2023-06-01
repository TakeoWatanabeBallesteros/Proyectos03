using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;       

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            transform.position = raycastHit.point;
        }
        
    }
    
}
