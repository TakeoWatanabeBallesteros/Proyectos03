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

    public VisualEffect waterWeakMesh;
    public VisualEffect waterStrongMesh;
    public VisualEffect particlesWater;
    private Blackboard_UIManager blackboardUI;
    public Transform weakWaterTransform;
    public Transform strongWaterTransform;
    public Transform mangueraObject;

    [SerializeField] private float weakWaterMeshScaleZ = 1f;
    [SerializeField] private float strongWaterMeshScaleZ = 1f;
    [SerializeField] float distanceHitPlayerWeak;
    [SerializeField] float distanceHitPlayerStrong;
    public float weakWaterVelocity = 1f; //Meters per second
    public float strongWaterVelocity = 1f; //Meters per second
    private Vector3 startPosition;
    public ParticleSystem weakSplash;
    public ParticleSystem strongSplash;


    private void OnDisable()
    {
        playerInput.controls.Player.Shoot.started -= ctx => ResetWater();
    }

    private void Start()
    {
        playerInput.controls.Player.Shoot.started += ctx => ResetWater();
        playerInput = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        fireExtinguish = GetComponent<FireExtinguish>();
        Kid = GetComponent<PickupKid>();
        _rb = GetComponent<Rigidbody>();
        canRecharge = false;
        WaterAmount = StartWater;
        timerKnockback = initialTimer;
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;

        waterWeakMesh.Play();
        weakWaterMeshScaleZ = 0f;
        waterWeakMesh.SetFloat("ScaleZ", weakWaterMeshScaleZ);
        waterStrongMesh.Play();
        strongWaterMeshScaleZ = 0f;
        waterStrongMesh.SetFloat("ScaleZ", strongWaterMeshScaleZ);
        startPosition = weakWaterTransform.localPosition;

        weakSplash.Stop();
        strongSplash.Stop();    

    }
    private void Update()
    {
        if (playerInput.shoot && WaterAmount > 0 && !playerInput.secondaryShoot)
        {
            StandardShootPerformed();
            if(fireExtinguish.distancePlayerRaycastHitWeak != fireExtinguish.WeakRayLenght && weakWaterMeshScaleZ >= distanceHitPlayerWeak - 1f) weakSplash.Play();
        }

        else if ((!playerInput.shoot || WaterAmount <= 0) && weakWaterMeshScaleZ > 0 && !playerInput.secondaryShoot)
        {
            weakSplash.Stop();
            StandardShootCancelled();
        }

        else weakWaterTransform.localPosition = startPosition; 

        if (playerInput.secondaryShoot && WaterAmount > 0 && !Kid.HasKid() && !playerInput.shoot)
        {
            StrongShootPerformed();
            StartCoroutine(KnockBackForce()); //Hay que tocar esto para mirar de tener un valor decente para el movimiento para atrás
            if (fireExtinguish.distancePlayerRaycastHitWeak != fireExtinguish.WeakRayLenght && strongWaterMeshScaleZ >= distanceHitPlayerStrong - 1f) strongSplash.Play();
        }

        else if ((!playerInput.secondaryShoot || WaterAmount <= 0) && strongWaterMeshScaleZ > 0 && !playerInput.shoot)
        {
            StrongShootCancelled();
            timerKnockback = initialTimer;
            strongSplash.Stop();
        }

        else strongWaterTransform.localPosition = startPosition; 

        if (playerInput.reacharge && canRecharge)
        {
            WaterAmount += waterReload * Time.deltaTime;
        }

        if (playerInput.interact)
        {
            UsingSecondary = false;
            StrongWater.Stop();
        }

        distanceHitPlayerWeak = fireExtinguish.distancePlayerRaycastHitWeak;
        distanceHitPlayerStrong = fireExtinguish.distancePlayerRaycastHitStrong;
    }

    private void StandardShootPerformed()
    {
        if (!particlesWater.GetSpawnSystemInfo("Spawn system").playing)
        {
            particlesWater.Play();
            particlesWater.SetFloat("Rate", 4f);
        }

        MoveWater(waterWeakMesh, weakWaterTransform, distanceHitPlayerWeak, weakWaterVelocity, ref weakWaterMeshScaleZ);
        ConsumeWater(NormalWaterConsumption);
    }

    private void StandardShootCancelled()
    {
        UsingPrimary = false;
        particlesWater.Stop();
        weakWaterMeshScaleZ = Mathf.Clamp(weakWaterMeshScaleZ - Time.deltaTime * weakWaterVelocity, 0, distanceHitPlayerWeak);
        waterWeakMesh.SetFloat("ScaleZ", weakWaterMeshScaleZ / 5);
        PointsBehavior.ResetCombo();
    }

    private void StrongShootPerformed()
    {
        UsingSecondary = true;
        if (!particlesWater.GetSpawnSystemInfo("Spawn system").playing)
        {
            particlesWater.Play();
            particlesWater.SetFloat("Rate", 6f);
        }
        StartCoroutine(StrongParticles());
    }

    IEnumerator StrongParticles()
    {
        PreWater.Play();
        yield return new WaitForSeconds(1f);
        PreWater.Stop();
        MoveWater( waterStrongMesh, strongWaterTransform, distanceHitPlayerStrong, strongWaterVelocity,ref strongWaterMeshScaleZ);
        ConsumeWater(StrongWaterConsumption);

    }
    void ConsumeWater(float WaterConsumption)
    {
        WaterAmount = Mathf.Clamp(WaterAmount - Time.deltaTime * WaterConsumption, 0, StartWater);
        blackboardUI.SetWaterBar(WaterAmount / 100);
    }

    private void StrongShootCancelled()
    {
        UsingSecondary = false;
        particlesWater.Stop();
        strongWaterMeshScaleZ = Mathf.Clamp(strongWaterMeshScaleZ - Time.deltaTime * strongWaterVelocity, 0, distanceHitPlayerStrong);
        waterStrongMesh.SetFloat("ScaleZ", strongWaterMeshScaleZ / 5);
        PointsBehavior.ResetCombo();
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

    void MoveWater(VisualEffect vfxmesh,Transform waterTransform,float maxDistance,float waterVelocity,ref float waterScaleZ)
    {
        Vector3 newPos = new Vector3(0, 0, Mathf.Clamp(waterTransform.localPosition.z + Time.deltaTime * waterVelocity, 0, maxDistance));
        waterTransform.localPosition = newPos;
        waterScaleZ = Mathf.Clamp(waterScaleZ + Time.deltaTime * waterVelocity, 0, maxDistance);
        vfxmesh.SetFloat("ScaleZ", waterScaleZ / 5);
    }

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

    private void ResetWater()
    {
        weakWaterTransform.localPosition = startPosition;
        //strongWaterTransform.localPosition = startPosition;
        weakWaterMeshScaleZ = 0f;
        //strongWaterMeshScaleZ = 0f;
        waterWeakMesh.SetFloat("ScaleZ", weakWaterMeshScaleZ / 5);
        //waterStrongMesh.SetFloat("ScaleZ", strongWaterMeshScaleZ / 5);
    }

}
