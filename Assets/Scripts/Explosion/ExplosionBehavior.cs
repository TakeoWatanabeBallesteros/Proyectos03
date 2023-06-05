using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

public class ExplosionBehavior : MonoBehaviour
{
    //private List<FireBehavior> nearObjectsOnFire = new List<FireBehavior>();
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
    
    [ContextMenu("Do Something")]
    void DoSomething()
    {
        StartCoroutine(Explode());
    }

    void Start()
    {
        camController = Camera.main.GetComponent<CameraController>();
        explosionParticles.gameObject.SetActive(false);
    }

    public IEnumerator Explode()
    {
        gameObject.tag = "Untagged";
        animator.SetTrigger(ExplodeId);
        yield return new WaitForSeconds(2f);
        explosionParticles.gameObject.SetActive(true);
        //CalculateExpansion();  
        ExplosionKnockBack();
        camController.shakeDuration = 1f;
        enabled = false;
    }
    // TODO: Revisar esto por si se puede hacer como antes 

    /*
    void CalculateExpansion()
    {
        foreach (var x in nearObjectsOnFire)
        {
            Debug.Log("Objeto alcanzado:" + x.name);
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (x.onFire) continue;
            if (distance <= closeRange) //if it's too close you get on fire instant
            {
                x.AddHeat(100);
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= midRange) //if it's between close and mid range then it's flammability increases
            {
                x.AddHeat(60);
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= highRange) //if it's between mid and far range then it's flammability increases
            {
                x.AddHeat(30);
                nearObjectsOnFire.Remove(x);
            }
        }
       
        for (int x = 0; x < nearObjectsOnFire.Count; x++)
        {
            Debug.Log(nearObjectsOnFire[x]);
            //Debug.Log("Objeto alcanzado:" + nearObjectsOnFire[x].name);
            float distance = Vector3.Distance(transform.position, nearObjectsOnFire[x].transform.position);

            if (nearObjectsOnFire[x].onFire) continue;
            nearObjectsOnFire[x].AddHeat(100);
            nearObjectsOnFire.Remove(nearObjectsOnFire[x]);
            
            if (distance <= closeRange) //if it's too close you get on fire instant
            {
                nearObjectsOnFire[x].AddHeat(100);
                nearObjectsOnFire.Remove(nearObjectsOnFire[x]);
            }            
            else if (distance <= midRange) //if it's between close and mid range then it's flammability increases
            {
                nearObjectsOnFire[x].AddHeat(60);
                nearObjectsOnFire.Remove(nearObjectsOnFire[x]);
            }
            else if (distance <= highRange) //if it's between mid and far range then it's flammability increases
            {
                nearObjectsOnFire[x].AddHeat(30);
                nearObjectsOnFire.Remove(nearObjectsOnFire[x]);
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
    */
    
    private void ExplosionKnockBack() //Apply force in x sphere radius
    {
        var colliders = Physics.OverlapSphere(transform.position, knockBackRadius, explosionMask);
        foreach (Collider target in colliders)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockBackRadius);
            target.gameObject.GetComponentInParent<FireBehavior>().AddHeat(100);
        }
    }

}

//how to make a solid disc to check radius from an object 
//
#if UNITY_EDITOR
[CustomEditor(typeof(ExplosionBehavior))]
public class HandlessDemoEditor : Editor
{
    public void OnSceneGUI()
    {
        var linkedObject = target as ExplosionBehavior;

        Handles.color = new Color(1, 0, 0, .3f);
        Handles.DrawSolidDisc(linkedObject.transform.position, Vector3.up, linkedObject.closeRange);
    }
}
#endif
