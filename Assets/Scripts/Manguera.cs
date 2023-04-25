using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    private bool canRecharge;
    public TMP_Text ChargeText;
    private PickupKid Kid;

    private void OnEnable()
    {
        primaryShoot.action.performed += StandardShootPerformed;
        primaryShoot.action.canceled += StandardShootCancelled;
        sencondaryShoot.action.performed += StrongShootPerformed;
        sencondaryShoot.action.canceled += StrongShootCancelled;
    }

    private void Start()
    {
        Kid = gameObject.GetComponent<PickupKid>();
        canRecharge = false;
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
        if (!UsingPrimary && !Kid.HasKid())
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
        if (canRecharge)
            WaterAmount = 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Recharge")
        {
            ChargeText.text = "Press E to Recharge Water";
            ChargeText.enabled = true;
            canRecharge = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Recharge")
        {
            ChargeText.enabled = false;
            canRecharge = false;
        }
    }
    private void OnDisable()
    {
        primaryShoot.action.performed -= StandardShootPerformed;
        primaryShoot.action.canceled -= StandardShootCancelled;
        sencondaryShoot.action.performed -= StrongShootPerformed;
        sencondaryShoot.action.canceled -= StrongShootCancelled;
    }
}
