using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class ObjectsExplosionv2 : MonoBehaviour
{
    private List<FirePropagation2> nearObjectsOnFire;
    public float closeRange;
    public float midRange;
    public float highRange;

    public Animator animator;

    public float knockBackRadius;
    public float explosionForce;
    public LayerMask explosionMask;
    public LayerMask playerMask;

    CameraController camController;
    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagation2>().ToList<FirePropagation2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        camController = Camera.main.GetComponent<CameraController>();
    }

    public IEnumerator Explode()
    {
        gameObject.tag = "Untagged";
        animator.SetTrigger("Explote");
        yield return new WaitForSeconds(2f);
        transform.GetChild(1).gameObject.SetActive(true);
        CalculateExpansion();
        ExplosionKnockBackCor();
        camController.shakeDuration = 1f;
        this.enabled = false;
    }
    
    void CalculateExpansion()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (x.onFire) continue;
            if (distance <= closeRange) //if it's too close you get on fire instant
            {
                x.transform.GetChild(0).gameObject.SetActive(true);
                x.onFire = true;
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= midRange) //if it's between close and mid range then it's flammability increases
            {
                x.IncrementHeat(60);
                nearObjectsOnFire.Remove(x);
            }
            else if (distance <= highRange) //if it's between mid and far range then it's flammability increases
            {
                x.IncrementHeat(30);
                nearObjectsOnFire.Remove(x);
            }
        }
    }  
    
    private void ExplosionKnockBackCor() //Apply force in x sphere radius
    {
        var colliders = Physics.OverlapSphere(transform.position, knockBackRadius, explosionMask);
        foreach (Collider target in colliders)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockBackRadius);

        }

        var playerCollider = Physics.OverlapSphere(transform.position, knockBackRadius, playerMask);
        foreach (Collider target in playerCollider)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockBackRadius);
        }
    }

}

//how to make a solid disc to check radius from an object 
//
#if UNITY_EDITOR
[CustomEditor(typeof(ObjectsExplosion))]
public class HandlessDemoEditor : Editor
{
    public void OnSceneGUI()
    {
        var linkedObject = target as ObjectsExplosion;

        Handles.color = new Color(1, 0, 0, .3f);
        Handles.DrawSolidDisc(linkedObject.transform.position, Vector3.up, linkedObject.closeRange);
    }
}
#endif
