using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChildrenHealthSystem : MonoBehaviour
{
    [SerializeField] float childHP = 100;
    public bool canDamaged = false;
    [SerializeField] float delay = 1;

    private PointsBehavior pointsManager;
    // Start is called before the first frame update
    void Start()
    {
        canDamaged = true;
        pointsManager = Singleton.Instance.PointsManager;
    }

    public void TakeDamage()
    {
        if (!canDamaged) return;
        childHP -= 10f;
        StartCoroutine(DamageCooldown());
        childHP = Mathf.Clamp(childHP, 0, 100);
        if (childHP<=0) Die();
    }

    public void StopBeingBurned()
    {
        StopAllCoroutines();
        canDamaged = true;
    }

    private IEnumerator DamageCooldown()
    {
        canDamaged = false;
        yield return new WaitForSeconds(delay);
        canDamaged = true;
    }

    private void Die()
    {
        pointsManager.RemovePointsChildBurned();
    }
}
