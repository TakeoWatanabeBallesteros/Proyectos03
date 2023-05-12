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

    //This script uses raycasts to detect the fire and send the order of extinguish it

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
        //This sets the weak water particles to a speed and lietime to reach the same length as the raycast
        var WeakMain = WeakWater.main;
        WeakMain.startLifetime = WeakRayLenght / WeakMain.startSpeed.constant;

        //Shoot a raycast for the weak water
        if (playerInput.shoot && !Manguera.GetSecondary() && Manguera.GetWaterAmount() > 0)
        {
            WeakWaterRaycast();
        }

        //Shoot the raycast for the strong water
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
        //Dont use the raycast because the player has stop pressing the button canceling the water shooting
        if (!playerInput.secondaryShoot)
        {
            SecondaryActivated = false;
            StopAllCoroutines();
        }

        //Delay for the raycast shooting for the strong water
        IEnumerator SecondaryDelay()
        {
            yield return new WaitForSeconds(1f);
            if (playerInput.secondaryShoot)
            {
                SecondaryActivated = true;
            }
        }

    }

  
    //Raycast Info and debuging
    //This activates a function on the fire script which causes it to lose HP over time
    private void WeakWaterRaycast()
    {
        Ray ray = new Ray(LasserOrigin.position, LasserOrigin.forward);
        Debug.DrawRay(LasserOrigin.position, LasserOrigin.forward* WeakRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, WeakRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponentInParent<FirePropagationV2>())
            {
                if (hit.collider.gameObject.GetComponentInParent<FirePropagationV2>().onFire)
                {
                    hit.collider.GetComponentInParent<FirePropagationV2>().TakeDamage(25);
                }
            }
            if (hit.collider.gameObject.GetComponentInParent<Collectable>())
            {
                hit.collider.GetComponentInParent<Collectable>().TakeDamage(25);
            }
        }
    }
    private void StrongWaterRaycast()
    {
        Ray ray = new Ray(LasserOrigin.position, LasserOrigin.forward);
        Debug.DrawRay(LasserOrigin.position, LasserOrigin.forward * StrongRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, StrongRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponentInParent<FirePropagationV2>())
            {
                if (hit.collider.gameObject.GetComponentInParent<FirePropagationV2>().onFire)
                {
                    hit.collider.GetComponentInParent<FirePropagationV2>().TakeDamage(50);
                }
            }
            if (hit.collider.gameObject.GetComponentInParent<Collectable>())
            {
                hit.collider.GetComponentInParent<Collectable>().TakeDamage(50);
            }
        }
    }
}
