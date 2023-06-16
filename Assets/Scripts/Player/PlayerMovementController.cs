using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private AnimatorController characterAnim;
    
    [HideInInspector]public bool canMove;
    
    public float Maxspeed;
    public float speed;
    float currentSpeed;
    //private CameraController mainCamera; pensaba que era un singleton
    
    private Vector3 direction;
    [SerializeField]private Rigidbody rb;
    [SerializeField] private PlayerInputController input;
    
    private Ray camRay;
    private Plane groundPlane;
    private float rayLength;
    private Vector3 pointToLook;

    Vector3 forward;
    Vector3 right;

    [SerializeField] private float gamePadSensitivity;

    [SerializeField] private float timeToRandomIdle;
    private float timeIdle;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        
        speed = Maxspeed;

        // Solo usar cuando disparas
        forward = _camera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = _camera.transform.right;
        right.y = 0;
        right.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove) return;
        
        direction = forward * input.movement.y + right * input.movement.x;
        if (input.shoot || input.secondaryShoot)
        {
            RotatePlayerShooting();
        }
        else{

            RotatePlayer();
   
        }
        if (input.movement != Vector2.zero)
        {
            timeIdle = 0;
            characterAnim.SetSpeed(1);
            MovePlayer(direction.normalized);
        }
        else
        {
            characterAnim.SetSpeed(0);
            currentSpeed = 0;
            timeIdle += Time.deltaTime;
        }
        rb.velocity = direction.normalized * currentSpeed;

        if (timeIdle >= timeToRandomIdle)
        {
            characterAnim.SetRandomIdle();
            timeIdle = 0;
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 10);
    }

    private void RotatePlayerShooting() {
        //Sending a raycast
        camRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

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

    private void RotatePlayer()
    {
        // Si no disparas usar este
        Vector3 movementDirection = input.movement.x * Vector3.right + input.movement.y * Vector3.forward;
        movementDirection = Quaternion.Euler(0, 45, 0) * movementDirection;
        // Vector3 realDirection = Camera.main.transform.TransformDirection(movementDirection);
        movementDirection.y = 0;
        // this line checks whether the player is making inputs.
        if (movementDirection.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
        }
    }

    public void KnockBack()
    {
        rb.AddForce(-transform.forward.normalized*0.1f);
    }

    public void Stop()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
    }
}
