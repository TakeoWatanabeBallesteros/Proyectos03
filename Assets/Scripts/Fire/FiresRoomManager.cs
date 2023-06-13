using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiresRoomManager : MonoBehaviour
{
    private List<FireBehavior> firesInRoom;
    [SerializeField] private bool playerInRoom;

    private void Awake()
    {
        firesInRoom = GetComponentsInChildren<FireBehavior>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerInRoom) return;
        foreach (var fire in firesInRoom)
        {
            fire?.FireUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRoom = true;
    }
}
