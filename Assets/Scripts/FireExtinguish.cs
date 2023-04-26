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

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = Player.GetComponent<InputPlayerController>();
        Kid = Player.GetComponent<PickupKid>();
        Manguera = Player.GetComponent<Manguera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.shoot && !Manguera.GetSecondary() && Manguera.GetWaterAmount() > 0)
        {
            WeakWaterRaycast();
        }
        else if (playerInput.secondaryShoot && !Manguera.GetPrimary() && Manguera.GetWaterAmount() > 0 && !Kid.HasKid())
        {
            StrongWaterRaycast();
        }

    }

    private void WeakWaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward* WeakRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, WeakRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponent<FirePropagationV2>().onFire)
            {
                hit.collider.GetComponent<FirePropagationV2>().TakeDamage();
                Debug.Log("normal");
            }
        }
    }
    private void StrongWaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward * StrongRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, StrongRayLenght, ColisionLayer))
        {
            if (hit.collider.gameObject.GetComponent<FirePropagationV2>().onFire)
            {
                hit.collider.GetComponent<FirePropagationV2>().TakeDamage();
                Debug.Log("fuerte");
            }
        }
    }
}
