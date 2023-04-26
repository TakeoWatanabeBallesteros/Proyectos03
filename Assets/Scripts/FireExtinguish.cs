using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguish : MonoBehaviour
{
    Vector3 mouseScreenPos;
    Vector3 mosuseWorldPos;
    public LayerMask GroundLayer;
    public LayerMask FireLayer;
    GameObject Player;
    public float WeakRayLenght;
    bool WeakHittingFire;
    public float StrongRayLenght;
    bool StrongHittingFire;
    public GameObject WeakFireTarget;
    public GameObject StrongFireTarget;
    [SerializeField] private InputPlayerController playerInput;
    PickupKid Kid;
    Manguera Manguera;

    // Start is called before the first frame update
    void Start()
    {
        WeakHittingFire = false;
        StrongHittingFire = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerInput = Player.GetComponent<InputPlayerController>();
        Kid = Player.GetComponent<PickupKid>();
    }

    // Update is called once per frame
    void Update()
    {
        PositionMouse();
        if (playerInput.shoot && !Manguera.GetPrimary() && !Manguera.GetSecondary() && Manguera.GetWaterAmount() > 0)
        {
            WeakWaterRaycast();
        }
        else if (playerInput.secondaryShoot && !Manguera.GetSecondary() && !Manguera.GetPrimary() && Manguera.GetWaterAmount() > 0 && !Kid.HasKid())
        {
            StrongWaterRaycast();
        }

    }

    private void WeakWaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward* WeakRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, WeakRayLenght, FireLayer))
        {
            WeakHittingFire = true;
            WeakFireTarget = hit.collider.gameObject;
            Debug.Log("normal");
        }
        else
            WeakHittingFire = false;        
    }
    private void StrongWaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward * StrongRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, StrongRayLenght, FireLayer))
        {
            StrongHittingFire = true;
            StrongFireTarget = hit.collider.gameObject;
            Debug.Log("fuerte");
        }
        else
            StrongHittingFire = false;
    }
    private void PositionMouse()
    {
        mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, GroundLayer))
        {
            mosuseWorldPos = hit.point;
        }

        transform.position = mosuseWorldPos;
    }
}
