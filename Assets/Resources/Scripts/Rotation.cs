using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    [SerializeField] private LayerMask groundMask;
    //[SerializeField] public List<float> rotations = new List<float>(4);

    float rotation;

    private Camera mainCamera;
    private GameObject mainCameraGO;

    private void Start()
    {
        // Cache the camera, Camera.main is an expensive operation.
        mainCamera = Camera.main;
        mainCameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        Aim();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            mainCameraGO.transform.Rotate(0f, 90f, 0f, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            mainCameraGO.transform.Rotate(0f, -90f, 0f, Space.World);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Aplicar la fuerza progressiva hacia el otro lado del ratón
        }
    }
    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            var direction = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }
    /*
    private void ForceApplication()
    {
        Ray l_Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_RayCastHit;
        Vector3 direction = m_Camera.transform.TransformDirection(new Vector3(0, 0, 1));
        VectorCam = m_Camera.transform.position - transform.position;

        if (Physics.Raycast(l_Ray, out l_RayCastHit, m_MaxShootDistance, m_WaterLayerMask.value))
        {

            if (l_RayCastHit.transform.tag == "CanMove")
            {
                l_RayCastHit.transform.position += direction * Force;
            }

            else
            {
                p_CharacterController.enabled = false;
                transform.position -= Vector3.Lerp(transform.position, direction * Force, 10f);
            }

        }
    }*/
}

    

