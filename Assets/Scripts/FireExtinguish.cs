using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguish : MonoBehaviour
{
    public LayerMask ColisionLayer;
    public LayerMask FireLayer;
    GameObject Player;
    public float WeakRayLenght;
    public float StrongRayLenght;
    [SerializeField] private InputPlayerController playerInput;
    PickupKid Kid;
    Manguera Manguera;
    bool SecondaryActivated;
    public ParticleSystem WeakWater;
    public Transform LasserOrigin;

    // Start is called before the first frame update
    void Start()
    {
        SecondaryActivated = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = Player.GetComponent<InputPlayerController>();
        Kid = Player.GetComponent<PickupKid>();
        Manguera = Player.GetComponent<Manguera>();


    }

    // Update is called once per frame
    void Update()
    {
        var WeakMain = WeakWater.main;
        WeakMain.startLifetime = WeakRayLenght / WeakMain.startSpeed.constant;
        if (playerInput.shoot && !Manguera.GetSecondary() && Manguera.GetWaterAmount() > 0)
        {
            WeakWaterRaycast();
        }

        if (playerInput.secondaryShoot && !Manguera.GetPrimary() && Manguera.GetWaterAmount() > 0 && !Kid.HasKid())
        {
            if (!SecondaryActivated)
            {
                StartCoroutine(SecondaryDelay());
            }
            else
            {
                StrongWaterRaycast();
            }
        }
        if (!playerInput.secondaryShoot)
        {
            SecondaryActivated = false;
            StopAllCoroutines();
        }

        IEnumerator SecondaryDelay()
        {
            yield return new WaitForSeconds(1f);
            if (playerInput.secondaryShoot)
            {
                SecondaryActivated = true;
            }
        }

    }

  

    private void WeakWaterRaycast()
    {
        Ray ray = new Ray(LasserOrigin.position, LasserOrigin.forward);
        Debug.DrawRay(LasserOrigin.position, LasserOrigin.forward* WeakRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, WeakRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponent<FirePropagationV2>().onFire)
            {
                hit.collider.GetComponent<FirePropagationV2>().TakeDamage(25);
            }
        }
    }
    private void StrongWaterRaycast()
    {
        Ray ray = new Ray(LasserOrigin.position, LasserOrigin.forward);
        Debug.DrawRay(LasserOrigin.position, LasserOrigin.forward * StrongRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, StrongRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponent<FirePropagationV2>().onFire)
            {
                hit.collider.GetComponent<FirePropagationV2>().TakeDamage(50);
            }
        }
    }
}
