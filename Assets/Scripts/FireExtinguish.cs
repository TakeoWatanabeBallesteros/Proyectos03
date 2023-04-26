using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguish : MonoBehaviour
{
    Vector3 mouseScreenPos;
    Vector3 mosuseWorldPos;
    public LayerMask LM;
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        WaterRaycast();
        PositionMouse();
    }

    private void WaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward*5);
    }
    private void PositionMouse()
    {
        mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LM))
        {
            mosuseWorldPos = hit.point;
        }

        transform.position = mosuseWorldPos;
    }
}
