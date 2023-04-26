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
    private GameObject Player;
    public float WeakRayLenght;
    private bool WeakHittingFire;
    public float StrongRayLenght;
    private bool StrongHittingFire;

    // Start is called before the first frame update
    void Start()
    {
        WeakHittingFire = false;
        StrongHittingFire = false;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        WeakWaterRaycast();
        StrongWaterRaycast();
        PositionMouse();
    }

    private void WeakWaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward* WeakRayLenght);
        if (Physics.Raycast(ray, out RaycastHit hit, WeakRayLenght, FireLayer))
        {
            WeakHittingFire = true;
            Debug.Log("weak");
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
            Debug.Log("STRONG");
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
    public bool GetWeakHit()
    {
        return WeakHittingFire;
    }
    public bool GetStrongHit()
    {
        return StrongHittingFire;
    }
}
