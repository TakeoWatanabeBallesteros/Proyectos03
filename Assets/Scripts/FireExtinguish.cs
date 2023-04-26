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
        Vector3 FixedPos = Pointer.transform.position;
        FixedPos.y = Player.transform.position.y;
        Ray ray = new Ray(Player.transform.position, FixedPos);
        Debug.DrawRay(Player.transform.position,FixedPos);
    }
}
