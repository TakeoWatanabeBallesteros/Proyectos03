using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayerController : MonoBehaviour
{
    private PlayerControls controls = null;
    
    public Vector2 movement { get; private set; }
    
    private void OnEnable()
    {
        if(controls != null)
            controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => movement = ctx.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
