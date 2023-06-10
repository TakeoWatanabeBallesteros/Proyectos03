using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

public class ExplosionBehavior : MonoBehaviour
{
    private List<FireBehavior> nearObjectsOnFire = new List<FireBehavior>();
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

    private FireBehavior fireBehavior;

    private PointsBehavior pointsManager;

    void Start()
    {
        camController = Camera.main.GetComponent<CameraController>();
        explosionParticles.gameObject.SetActive(false);
        fireBehavior = GetComponent<FireBehavior>();
        pointsManager = Singleton.Instance.PointsManager;
    }

    public IEnumerator Explode()
    {
        gameObject.tag = "Untagged";
        animator.SetTrigger(ExplodeId);
        yield return new WaitForSeconds(2f);
        explosionParticles.gameObject.SetActive(true);
        pointsManager.AddPointsExplosion();
        CalculateExpansion();  
        ExplosionKnockBack();
        camController.shakeDuration = 1f;
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            nearObjectsOnFire.Add(other.GetComponentInParent<FireBehavior>());
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
