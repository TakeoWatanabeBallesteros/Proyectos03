using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Manguera : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference primaryShoot, sencondaryShoot;
    private bool UsingPrimary = false;
    private bool UsingSecondary = false;
    public ParticleSystem PreWater;
    public ParticleSystem StrongWater;
    public ParticleSystem WeakWater;
    public Slider WaterBar;
    private float WaterAmount;
    public float NormalWaterConsumption;
    public float StrongWaterConsumption;

    private void OnEnable()
    {
        primaryShoot.action.performed += StandardShootPerformed;
        primaryShoot.action.canceled += StandardShootCancelled;
        sencondaryShoot.action.performed += StrongShootPerformed;
        sencondaryShoot.action.canceled += StrongShootCancelled;
    }

    private void Start()
    {
        WaterAmount = 1;
    }

    private void StandardShootPerformed(InputAction.CallbackContext obj)
    {
        if (!UsingSecondary)
        {
            UsingPrimary = true;
            WeakWater.Play();
            StartCoroutine(ConsumeWater(NormalWaterConsumption));
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
            StartCoroutine(ConsumeWater(StrongWaterConsumption));
        }
    }
    IEnumerator ConsumeWater(float WaterConsumtion)
    {
        yield return new WaitForSeconds(.1f);
        WaterAmount -= WaterConsumtion;
        if (UsingPrimary || UsingSecondary)
        {
            StartCoroutine(ConsumeWater(WaterConsumtion));
        }
    }

    private void StrongShootCancelled(InputAction.CallbackContext obj)
    {
        UsingSecondary = false;
        StrongWater.Stop();
    }

    private void Update()
    {
        WaterBar.value = WaterAmount;

    }

    private void OnInteract(InputValue valor)
    {
        WaterAmount = 1;
    }

        private void OnDisable()
    {
        primaryShoot.action.performed -= StandardShootPerformed;
        primaryShoot.action.canceled -= StandardShootCancelled;
        sencondaryShoot.action.performed -= StrongShootPerformed;
        sencondaryShoot.action.canceled -= StrongShootCancelled;
    }
}
