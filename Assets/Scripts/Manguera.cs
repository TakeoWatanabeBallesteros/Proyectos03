using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Manguera : MonoBehaviour
{
    [SerializeField] private InputPlayerController playerInput;
    bool UsingPrimary = false;
    bool UsingSecondary = false;
    public ParticleSystem PreWater;
    public ParticleSystem StrongWater;
    public ParticleSystem WeakWater;
    public Slider WaterBar;
    float WaterAmount;
    public float NormalWaterConsumption;
    public float StrongWaterConsumption;
    bool canRecharge;
    public TMP_Text ChargeText;
    PickupKid Kid;
    [SerializeField] private float StartWater;

    private void Start()
    {
        playerInput = GetComponent<InputPlayerController>();
        Kid = GetComponent<PickupKid>();
        canRecharge = false;
        WaterAmount = StartWater;
    }
    private void Update()
    {
        if (playerInput.shoot && !UsingPrimary && !UsingSecondary && WaterAmount > 0)
        {
            StandardShootPerformed();
        }
        else if (!playerInput.shoot && UsingPrimary)
        {
            StandardShootCancelled();
        }

        if (playerInput.secondaryShoot && !UsingSecondary && !UsingPrimary && WaterAmount > 0 && !Kid.HasKid())
        {
            StrongShootPerformed();
        }
        else if(!playerInput.secondaryShoot && UsingSecondary)
        {
            StrongShootCancelled();
        }

        WaterBar.value = WaterAmount;
        if (WaterAmount < 0)
        {
            WeakWater.Stop();
            StrongWater.Stop();
        }
    }

    private void StandardShootPerformed()
    {
        UsingPrimary = true;
        WeakWater.Play();
        StartCoroutine(ConsumeWater(NormalWaterConsumption));

    }

    private void StandardShootCancelled()
    {
        UsingPrimary = false;
        WeakWater.Stop();
    }

    private void StrongShootPerformed()
    {
        UsingSecondary = true;
        StartCoroutine(StrongParticles());
    }

    IEnumerator StrongParticles()
    {
        PreWater.Play();
        yield return new WaitForSeconds(1f);
        if (UsingSecondary == true && WaterAmount > 0)
        {
            StrongWater.Play();
            StartCoroutine(ConsumeWater(StrongWaterConsumption));
        }
    }
    IEnumerator ConsumeWater(float WaterConsumtion)
    {
        yield return new WaitForSeconds(.1f);
        WaterAmount -= WaterConsumtion;
        if (WaterAmount > 0 && (UsingPrimary || UsingSecondary))
        {
            StartCoroutine(ConsumeWater(WaterConsumtion));
        }
    }

    private void StrongShootCancelled()
    {
        UsingSecondary = false;
        StrongWater.Stop();
    }


    private void OnRecharge(InputValue valor)
    {
        if (canRecharge)
            WaterAmount = 1;
    }
    private void OnInteract()
    {
        UsingSecondary = false;
        StrongWater.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Recharge")
        {
            ChargeText.text = "Press R to Recharge Water";
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
    public bool GetPrimary()
    {
        return UsingPrimary;
    }
    public bool GetSecondary()
    {
        return UsingSecondary;
    }
    public float GetWaterAmount()
    {
        return WaterAmount;
    }

}
