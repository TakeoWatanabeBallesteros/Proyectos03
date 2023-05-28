using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class Manguera : MonoBehaviour
{
    [SerializeField] private InputPlayerController playerInput;
    private MovementPlayerController playerMovement;
    bool UsingPrimary = false;
    bool UsingSecondary = false;
    public ParticleSystem PreWater;
    public ParticleSystem StrongWater;
    public ParticleSystem WeakWater;
    public Slider WaterBar;
    [SerializeField] float WaterAmount;
    public float NormalWaterConsumption;
    public float StrongWaterConsumption;
    bool canRecharge;
    public TMP_Text ChargeText;
    PickupKid Kid;
    [SerializeField] private float StartWater;
    [SerializeField] private float waterReload; // The increas of water per second
    bool isReloading = false;

    Rigidbody _rb;
    [SerializeField] float timerKnockback;
    float initialTimer = 0f;

    public float divisionForce;
    public float waitTime;
    private void Start()
    {
        playerInput = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        Kid = GetComponent<PickupKid>();
        _rb = GetComponent<Rigidbody>();
        canRecharge = false;
        WaterAmount = StartWater;
        timerKnockback = initialTimer;
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
            StartCoroutine(KnockBackForce());
        }

        else if (!playerInput.secondaryShoot && UsingSecondary)
        {
            StrongShootCancelled();
            timerKnockback = initialTimer;
        }

        WaterBar.value = WaterAmount;

        if (WaterAmount < 0)
        {
            WeakWater.Stop();
            StrongWater.Stop();
        }

        if (playerInput.reacharge && canRecharge)
        {
            //isReloading = true;
            WaterAmount += waterReload * Time.deltaTime;
            //StartCoroutine(Recharge());
        }

        if (playerInput.interact)
        {
            UsingSecondary = false;
            StrongWater.Stop();
        }

        WaterAmount = Mathf.Clamp(WaterAmount, 0f, 1f);
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
            //StartCoroutine(AddForce());

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

    /*
    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(1f);
        forceAdded = true;  
        _rb.AddForce(-transform.forward.normalized/100000, ForceMode.Impulse); //Preguntar como hacer lerp para que quede smooth
        yield return new WaitForSeconds(1.5f);
        forceAdded = false;
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position - (transform.forward * knockbackForce), 2f);
        Vector3 newPosition = new Vector3(transform.position.x - (transform.forward.x * knockbackForce), transform.position.y, transform.position.z - (transform.forward.z * knockbackForce));
        transform.position = Vector3.Lerp(transform.position, newPosition, 2f);        
    }*/

    IEnumerator KnockBackForce()
    {        
        yield return new WaitForSeconds(1f);

        while (timerKnockback < waitTime)
        {
            timerKnockback += Time.deltaTime;

            if (UsingSecondary == true)
            {
                _rb.AddForce(-transform.forward / divisionForce * Time.deltaTime, ForceMode.Impulse);
            }

            yield return new WaitForFixedUpdate();
        }

    }
    /*
    IEnumerator Recharge()
    {
        float waitTime = 2f;
        float timerRecharging = 0f;

        while (timerRecharging < waitTime)
        {
            timerRecharging += Time.deltaTime;
            WaterAmount = Mathf.Lerp(WaterAmount, 1, timerRecharging/waitTime);
            yield return null;
        }
    }*/
}
