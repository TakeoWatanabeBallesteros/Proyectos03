using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MovementPlayerController : MonoBehaviour
{
    public float speed;
    public Camera cam;
    
    private Rigidbody rb;
    private Vector3 direction;
    private InputPlayerController input;
    
    private Ray camRay;
    private Plane groundPlane;
    private float rayLength;
    private Vector3 pointToLook;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<InputPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        
        direction = Vector3.zero;
        direction = (transform.right * input.movement.y) + (transform.forward * input.movement.x);
    }

    private void FixedUpdate()
    {
        MovePlayer(direction);
    }

    private void MovePlayer(Vector3 direction)
    {
        rb.velocity = this.direction * speed;
    }

    private void RotatePlayer()
    {
        //Sending a raycast
        camRay = cam.ScreenPointToRay(Input.mousePosition);

        //Setting groundPlane
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        //Checking if the ray hit something
        if (groundPlane.Raycast(camRay, out rayLength))
        {
            pointToLook = camRay.GetPoint(rayLength);
        }

        //Rotating the player
        transform.LookAt(new Vector3(pointToLook.x, pointToLook.y, pointToLook.z));
    }
}