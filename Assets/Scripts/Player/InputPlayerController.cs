using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputPlayerController : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    
    public Vector2 movement { get; private set; }
    public bool shoot { get; private set; }
    public bool interact { get; private set; }
    public bool secondaryShoot { get; private set; }
    public bool reacharge { get; private set; }
    public Vector2 zoom { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Shoot.performed += ctx => shoot = ctx.ReadValueAsButton();
        controls.Player.Shoot.canceled += ctx => shoot = ctx.ReadValueAsButton(); 
        controls.Player.SecondaryShot.performed += ctx => secondaryShoot = ctx.ReadValueAsButton();
        controls.Player.SecondaryShot.canceled += ctx => secondaryShoot = ctx.ReadValueAsButton();
        controls.Player.Interact.performed += ctx => interact = ctx.ReadValueAsButton();
        controls.Player.Interact.canceled += ctx => interact = ctx.ReadValueAsButton();
        controls.Player.Recharge.performed += ctx => reacharge = ctx.ReadValueAsButton();
        controls.Player.Recharge.canceled += ctx => reacharge = ctx.ReadValueAsButton();
        controls.Player.Zoom.performed += ctx => zoom = ctx.ReadValue<Vector2>();
        controls.Player.Zoom.canceled += ctx => zoom = ctx.ReadValue<Vector2>();
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
