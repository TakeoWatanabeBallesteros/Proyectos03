using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_FireZone : MonoBehaviour
{
    public List<GameObject> fireObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject fireObject in fireObjects)
            {
                FireBehavior fireBehavior = fireObject.GetComponent<FireBehavior>();
                if (fireBehavior != null)
                {
                    fireBehavior.AddHeat(100f);
                }
            }

        }
    }
}

