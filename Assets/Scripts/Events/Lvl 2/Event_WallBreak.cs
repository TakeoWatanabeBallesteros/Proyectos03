using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using UnityEditor;
using UnityEngine;

public class Event_WallBreak : MonoBehaviour
{
    public GameObject prefabToActivate;
    public GameObject prefabToDeactivate;
    public GameObject explosionParticles;
    public GameObject explosionPrefab;

    private void Start()
    {
        explosionParticles.SetActive(false);
    }

    public void BreakWall()
    {
        if (prefabToActivate != null && prefabToDeactivate != null)
        {
            prefabToActivate.SetActive(true);
            explosionParticles.SetActive(true);
            prefabToDeactivate.SetActive(false);
            StartCoroutine(DeactivateExploder());

        }
        else
        {
            Debug.LogError("Prefabs are not assigned properly.");
        }
    }

    public IEnumerator DeactivateExploder()
    {
        yield return new WaitForSeconds(0.05f);
        explosionPrefab.SetActive(false);
    }

}
