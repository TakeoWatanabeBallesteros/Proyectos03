using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KidHealthBehavior : MonoBehaviour, IHealth
{
    public KidType kidType;
    [SerializeField] private bool canBeDamaged = false;
    [SerializeField] float delay = 1;
    public float _maxHealth;

    private PointsBehavior pointsManager;

    private ItemManager itemsManager;
    // Start is called before the first frame update
    void Start()
    {
        canBeDamaged = true;
        pointsManager = Singleton.Instance.PointsManager;
        itemsManager = GameObject.FindWithTag("ItemsManager").GetComponent<ItemManager>();
        health = maxHealth;
    }

    public float health { get; set; }
    public float maxHealth
    {
        get {return _maxHealth; }
    }
    public Vector3 position
    {
        get { return transform.position; }
    }

    public void TakeDamage(float damage)
    {
        if (!canBeDamaged) return;
        StartCoroutine(DamageCooldown());
        health = Mathf.Clamp(health -= damage, 0, maxHealth);
        if (health == 0) Die();
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
        itemsManager.DeadChild();
        enabled = false;
        this.transform.parent.gameObject.SetActive(false);
    }
}