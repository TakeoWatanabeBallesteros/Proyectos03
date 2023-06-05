using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChildrenHealthSystem : MonoBehaviour
{

    [SerializeField] float childHP = 100;
    public bool canDamaged = false;
    [SerializeField] float delay = 1;
    public bool isGettingBurned;
    // Start is called before the first frame update
    void Start()
    {
        isGettingBurned = false;
        canDamaged = true;
    }

    public void TakeDamage()
    {
        isGettingBurned = true;
        if (!canDamaged)
        {
            childHP -= 10f;
            StartCoroutine(DamageCooldown());
        }
    }

    public void StopBeingBurned()
    {
        StopAllCoroutines();
        canDamaged = true;
        isGettingBurned = false;
    }

    private IEnumerator DamageCooldown()
    {
        canDamaged = false;
        yield return new WaitForSeconds(delay);
        canDamaged = true;
    }
}
