using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manguera : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference primaryShoot, sencondaryShoot;
    [SerializeField] private bool UsingPrimary = false;
    [SerializeField] private bool UsingSecondary = false;
    public ParticleSystem PreWater;
    public ParticleSystem StrongWater;
    public ParticleSystem WeakWater;

    private void OnEnable()
    {
        primaryShoot.action.performed += StandardShootPerformed;
        primaryShoot.action.canceled += StandardShootCancelled;
        sencondaryShoot.action.performed += StrongShootPerformed;
        sencondaryShoot.action.canceled += StrongShootCancelled;
    }
    private void StandardShootPerformed(InputAction.CallbackContext obj)
    {
        if (!UsingSecondary)
        {
            UsingPrimary = true;
            WeakWater.Play();
        }
    }

    private void StandardShootCancelled(InputAction.CallbackContext obj)
    {
        UsingPrimary = false;
        WeakWater.Stop();
    }

    private void StrongShootPerformed(InputAction.CallbackContext obj)
    {
        if (!UsingPrimary)
        {
            UsingSecondary = true;
            StartCoroutine(StrongParticles());
        }
    }

    IEnumerator StrongParticles()
    {
        PreWater.Play();
        yield return new WaitForSeconds(1f);
        if (UsingSecondary == true)
        {
            StrongWater.Play();
        }
    }

    private void StrongShootCancelled(InputAction.CallbackContext obj)
    {
        UsingSecondary = false;
        StrongWater.Stop();
    }
    private void OnDisable()
    {
        primaryShoot.action.performed -= StandardShootPerformed;
        primaryShoot.action.canceled -= StandardShootCancelled;
        sencondaryShoot.action.performed -= StrongShootPerformed;
        sencondaryShoot.action.canceled -= StrongShootCancelled;
    }
}
