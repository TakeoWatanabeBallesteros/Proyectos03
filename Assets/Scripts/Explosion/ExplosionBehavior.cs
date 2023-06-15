using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

public class ExplosionBehavior : MonoBehaviour
{
    private List<FireBehavior> nearObjectsOnFire = new List<FireBehavior>();
    private List<IHealth> healthEntities = new List<IHealth>();
    public float closeRange;
    public float midRange;
    public float highRange;

    public Animator animator;

    public float knockBackRadius;
    public float explosionForce;
    public LayerMask explosionMask;

    CameraController camController;
    private static readonly int ExplodeId = Animator.StringToHash("Explode");
    public GameObject explosionParticles;
    public GameObject electricParticles;
    public Event_WallBreak wallBreakEvent;

    private FireBehavior fireBehavior;

    private PointsBehavior pointsManager;

    public Collider zoneExpansionCollider;
    public Collider detectionCollider;
    
    [ContextMenu("Do Something")]
    void DoSomething()
    {
        MakeItExplote();
    }

    void Start()
    {
        camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        explosionParticles.gameObject.SetActive(false);
        fireBehavior = GetComponent<FireBehavior>();
        pointsManager = Singleton.Instance.PointsManager;
        detectionCollider.enabled = true;
        zoneExpansionCollider.enabled = false;
        electricParticles.gameObject.SetActive(true);
    }

    public void MakeItExplote()
    {
        gameObject.tag = "Untagged";
        animator.SetTrigger(ExplodeId);
        StartCoroutine(StartExplosion());
        electricParticles.gameObject.SetActive(false);
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSecondsRealtime(1.75f);
        detectionCollider.enabled = false;
        zoneExpansionCollider.enabled = true;
        yield return new WaitForSecondsRealtime(.25f);
        explosionParticles.gameObject.SetActive(true);
        if(wallBreakEvent != null) wallBreakEvent.BreakWall();
        pointsManager.AddPointsExplosion();
        camController.shakeDuration = 1f;
        CalculateExpansion();  
        ExplosionKnockBack();
        fireBehavior.enabled = true;
        enabled = false;
    }
    
    void CalculateExpansion()
    {
        List<FireBehavior> clone = new List<FireBehavior>(nearObjectsOnFire);
        foreach (var x in clone)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (x.onFire) continue;
            if (distance <= closeRange)
            {
                x.AddHeat(100);
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= midRange)
            {
                x.AddHeat(60);
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= highRange)
            {
                x.AddHeat(30);
                nearObjectsOnFire.Remove(x);
            }
        }

        foreach (var x in healthEntities.ToList())
        {
            float distance = Vector3.Distance(transform.position, x.position);
            
            if (x.health <= 0) continue;
            if (distance <= closeRange)
            {
                x.TakeDamage(100);
                healthEntities.Remove(x);
            }
            else if (distance <= midRange)
            {
                x.TakeDamage(60);
                healthEntities.Remove(x);
            }
            else if (distance <= highRange)
            {
                x.TakeDamage(30);
                healthEntities.Remove(x);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            var fire = other.GetComponentInParent<FireBehavior>();
            if(!fire.onFire) nearObjectsOnFire.Add(other.GetComponentInParent<FireBehavior>());
        }

        else if (other.TryGetComponent<IHealth>(out var health) && health.health > 0f)
        {
            healthEntities.Add(health);
        }

        else if (other.CompareTag("Explosive"))
        {
            other.GetComponent<ExplosionBehavior>().MakeItExplote();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            nearObjectsOnFire.Remove(other.GetComponentInParent<FireBehavior>());
        }
    }
    
    private void ExplosionKnockBack() //Apply force in x sphere radius
    {
        var colliders = Physics.OverlapSphere(transform.position, knockBackRadius, explosionMask);
        foreach (Collider target in colliders)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockBackRadius);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, closeRange);
        Gizmos.color = new Color(1, 0.57f, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, midRange);
        Gizmos.color = new Color(1, 0.97f, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, highRange);
    }
}
