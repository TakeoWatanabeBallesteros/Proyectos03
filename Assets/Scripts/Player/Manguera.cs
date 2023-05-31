using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using UnityEngine.VFX;

public class Manguera : MonoBehaviour
{
    [SerializeField] private InputPlayerController playerInput;
    private MovementPlayerController playerMovement;
    [SerializeField] private FireExtinguish fireExtinguish;
    bool UsingPrimary = false;
    bool UsingSecondary = false;
    public ParticleSystem PreWater;
    public ParticleSystem StrongWater;
    public ParticleSystem WeakWater;
    [SerializeField] float WaterAmount;
    public float NormalWaterConsumption; //Litres per second
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

    public VisualEffect waterMesh;
    public VisualEffect particlesWater;
    private Blackboard_UIManager blackboardUI;
    public Transform waterObject;

    [SerializeField] private float waterMeshScaleZ = 1f;
    [SerializeField] float distanceHitPlayer;
    public float waterVelocity = 1f; //Meters per second
    private Vector3 startPosition;

    private void Start()
    {
        playerInput = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        fireExtinguish = GetComponent<FireExtinguish>();
        Kid = GetComponent<PickupKid>();
        _rb = GetComponent<Rigidbody>();
        canRecharge = false;
        WaterAmount = StartWater;
        timerKnockback = initialTimer;
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        waterMesh.Play();
        waterMeshScaleZ = 0f;
        waterMesh.SetFloat("ScaleZ", waterMeshScaleZ);
        startPosition = waterObject.localPosition;
    }
    private void Update()
    {
        if (playerInput.shoot && WaterAmount > 0 && !playerInput.secondaryShoot)
        {
            StandardShootPerformed();
        }

        else if ((!playerInput.shoot || WaterAmount <= 0) && waterMeshScaleZ > 0)
        {
            StandardShootCancelled();
        }

        else waterObject.localPosition = startPosition;

        if (playerInput.secondaryShoot && WaterAmount > 0 && !Kid.HasKid() && !playerInput.shoot)
        {
            StrongShootPerformed();
            StartCoroutine(KnockBackForce());
        }

        else if (!playerInput.secondaryShoot)
        {
            StrongShootCancelled();
            timerKnockback = initialTimer;
        }
               

        if (WaterAmount < 0)
        {
            WeakWater.Stop();
            StrongWater.Stop();
            particlesWater.Stop();
            //waterMesh.Stop();
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

        distanceHitPlayer = fireExtinguish.distancePlayerRaycastHit;        
    }

    private void StandardShootPerformed()
    {
        particlesWater.playRate = 2f;
        //WeakWater.Play();
        //waterMesh.Play(); 
        particlesWater.Play();
        ConsumeWater(NormalWaterConsumption);
        MoveWater();

    }

    private void StandardShootCancelled()
    {
        UsingPrimary = false;
        //WeakWater.Stop();
        particlesWater.Stop();
        waterMeshScaleZ = Mathf.Clamp(waterMeshScaleZ - Time.deltaTime * waterVelocity, 0, fireExtinguish.distancePlayerRaycastHit);
        waterMesh.SetFloat("ScaleZ", waterMeshScaleZ/5);
    }

    private void StrongShootPerformed()
    {
        UsingSecondary = true;
        particlesWater.playRate = 3f;
        particlesWater.Play();
        StartCoroutine(StrongParticles());
    }

    IEnumerator StrongParticles()
    {
        PreWater.Play();
        yield return new WaitForSeconds(1f);
        if (UsingSecondary == true && WaterAmount > 0)
        {
            StrongWater.Play();
            ConsumeWater(StrongWaterConsumption);
        }

    }
    void ConsumeWater(float WaterConsumption)
    {
        WaterAmount = Mathf.Clamp(WaterAmount - Time.deltaTime * WaterConsumption, 0, StartWater);
        blackboardUI.SetWaterBar(WaterAmount/100);
    }

    private void StrongShootCancelled()
    {
        UsingSecondary = false;
        StrongWater.Stop(); 
        //waterMesh.Stop();
        particlesWater.Stop();
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

    void MoveWater()
    {
        Vector3 newPos = new Vector3(0, 0, Mathf.Clamp(waterObject.localPosition.z + Time.deltaTime * waterVelocity, 0, fireExtinguish.distancePlayerRaycastHit));
        waterObject.localPosition = newPos;
        waterMeshScaleZ = Mathf.Clamp(waterMeshScaleZ + Time.deltaTime * waterVelocity, 0, fireExtinguish.distancePlayerRaycastHit);
        waterMesh.SetFloat("ScaleZ", waterMeshScaleZ/5);
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
