using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireExtinguish : MonoBehaviour
{
    private GameObject Player;
    public GameObject Pointer;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        WaterRaycast();
    }

    private void WaterRaycast()
    {
        Ray ray = new Ray(Player.transform.position, Player.transform.forward);
        Debug.DrawRay(Player.transform.position, Player.transform.forward*5);
    }
}
