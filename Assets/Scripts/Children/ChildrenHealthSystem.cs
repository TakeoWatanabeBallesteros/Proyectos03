using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChildrenHealthSystem : MonoBehaviour
{
    [SerializeField] float childHP = 100;
    [SerializeField] private bool canBeDamaged = false;
    [SerializeField] float delay = 1;

    private PointsBehavior pointsManager;
    // Start is called before the first frame update
    void Start()
    {
        canBeDamaged = true;
        pointsManager = Singleton.Instance.PointsManager;
    }

    public void TakeDamage()
    {
        if (!canBeDamaged) return;
        childHP -= 10f;
        StartCoroutine(DamageCooldown());
        childHP = Mathf.Clamp(childHP, 0, 100);
        if (childHP == 0) Die();
    }

    public void StopBeingBurned()
    {
        StopAllCoroutines();
        canBeDamaged = true;
    }

    private IEnumerator DamageCooldown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(delay);
        canBeDamaged = true;
    }

    private void Die()
    {
        pointsManager.RemovePointsChildBurned();
        enabled = false;
    }
}
