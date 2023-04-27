using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayerController : MonoBehaviour
{
    public float speed;
    float currentSpeed;
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
        /// Solo usar cuando disparas
        // RotatePlayer();
        
        direction = Vector3.zero;
        if (input.shoot || input.secondaryShoot)
        {
            // Solo usar cuando disparas
            Vector3 forward = cam.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = cam.transform.right;
            right.y = 0;
            right.Normalize();

            direction = forward * input.movement.y + right * input.movement.x;
            RotatePlayer();
        }
        if(input.movement != Vector2.zero)
        {
            if(!input.shoot && !input.secondaryShoot)
            {
                // Si no disparas usar este
                Vector3 movementDirection = new Vector3(input.movement.x, 0, input.movement.y);
                Vector3 realDirection = Camera.main.transform.TransformDirection(movementDirection);
                realDirection.y = 0;
                // this line checks whether the player is making inputs.
                if(realDirection.magnitude > 0.1f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(realDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
                }
                direction = transform.forward.normalized;
            }
            

        }
        else
        {
           currentSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(direction.normalized);
        // ExperimentalMove();
    }

    private void MovePlayer(Vector3 direction)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 10);
        rb.velocity = direction * currentSpeed;
    }

    private void ExperimentalMove()
    {
        
    }

    private void RotatePlayer()
    {
        //Sending a raycast
        camRay = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        //Setting groundPlane
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        //Checking if the ray hit something
        if (groundPlane.Raycast(camRay, out rayLength))
        {
            pointToLook = camRay.GetPoint(rayLength);
        }

        //Rotating the player
        var targetRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, transform.position.y, pointToLook.z) - transform.position);;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    public void KnockBack()
    {
        rb.AddForce(-transform.forward.normalized*0.1f);
    }
}
