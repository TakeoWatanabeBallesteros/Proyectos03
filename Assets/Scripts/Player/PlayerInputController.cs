using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    public Vector2 movement { get; private set; }
    public Vector2 look { get; private set; }
    public bool shoot { get; private set; }
    public bool interact { get; private set; }
    public bool secondaryShoot { get; private set; }
    public bool esc { get; private set; }
    public bool space { get; private set; }

    private void OnEnable()
    {
        controls = controls ?? new PlayerControls();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        controls = controls ?? new PlayerControls();
        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => movement = Vector2.zero;
        controls.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => look = Vector2.zero;
        controls.Player.Shoot.performed += ctx => shoot = ctx.ReadValueAsButton();
        controls.Player.Shoot.canceled += ctx => shoot = ctx.ReadValueAsButton(); 
        controls.Player.Throw.performed += ctx => secondaryShoot = ctx.ReadValueAsButton();
        controls.Player.Throw.canceled += ctx => secondaryShoot = ctx.ReadValueAsButton();
        controls.Player.Interact.performed += ctx => interact = ctx.ReadValueAsButton();
        controls.Player.Interact.canceled += ctx => interact = ctx.ReadValueAsButton();
        controls.Player.Space.started += ctx => space = ctx.ReadValueAsButton();
        controls.Player.Space.canceled += ctx => space = ctx.ReadValueAsButton();
        controls.Player.Esc.started += ctx => esc = ctx.ReadValueAsButton();
        controls.Player.Esc.canceled += ctx => esc = ctx.ReadValueAsButton();
    }

    #region Add & Remove Functions
    public void AddShootFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Shoot.started += function;
    }
    public void AddShootCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Shoot.canceled += function;
    }
    public void RemoveShootFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Shoot.started -= function;
    }
    public void RemoveShootCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Shoot.canceled -= function;
    }
    public void AddThrowFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Throw.started += function;
    }
    public void AddThrowCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Throw.canceled += function;
    }
    public void RemoveThrowFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Throw.started -= function;
    }
    public void RemoveThrowCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Throw.canceled -= function;
    }
    public void AddInteractFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Interact.performed += function;
    }
    public void RemoveInteractFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Interact.performed -= function;
    }
    public void AddRechargeFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Recharge.started += function;
    }
    public void RemoveRechargeFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Recharge.started -= function;
    }
    public void AddRechargeCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Recharge.canceled += function;
    }
    public void RemoveRechargeCanceledFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Recharge.canceled -= function;
    }
    public void AddSpaceFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Space.started += function;
    }
    public void RemoveSpaceFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Space.canceled -= function;
    }
    public void AddEscFunction(Action<InputAction.CallbackContext> function) {
        controls.Player.Esc.started += function;
    }
    #endregion
}
