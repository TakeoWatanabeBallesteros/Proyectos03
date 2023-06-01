using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class ObjectsExplosionv2 : MonoBehaviour
{
    // Start is called before the first frame update

    public bool doExplote = false;
    public bool preExplosion = false;
    public List<FirePropagation2> nearObjectsOnFire = new List<FirePropagation2>();
    public List<FirePropagation2> closestObjects = new List<FirePropagation2>();
    public List<FirePropagation2> secondObjects = new List<FirePropagation2>();
    public List<FirePropagation2> farestObjects = new List<FirePropagation2>();

    public float closeRange;
    public float midRange;
    public float highRange;

    float expansionTimer;
    public float delay;
    float expansionExplosionTimer;
    public float delayExplosionTimer;

    public float maxRangeExplosion;

    public Animator animator;

    public float knockbackRadius;
    public float explosionForce;
    [SerializeField] Collider[] colliders;
    [SerializeField] Collider[] playerCollider;
    public LayerMask explosionMask;
    public LayerMask playerMask;

    public bool isOneLoopDone = false;

    CameraController camController;
    public bool doExplosion = false;
    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagation2>().ToList<FirePropagation2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        expansionExplosionTimer = delayExplosionTimer;
        camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOneLoopDone)
        {
            if (preExplosion)
            {
                animator.SetTrigger("Explote");
                CalculateExpansion();
                PointsBehavior.AddPointsExplosion();
            }

            if (doExplote && expansionExplosionTimer >= 0f)
            {
                Debug.Log("Expansion on explosion");
                preExplosion = false;
                expansionExplosionTimer -= Time.deltaTime;
                
                /*
                if (expansionTimer >= 0f)
                {
                    expansionTimer -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Expansion");
                    expansionTimer = delay;
                    
                }*/
            }
            else
            {
                expansionExplosionTimer = delayExplosionTimer;
                doExplote = false;
            }

        }  
        
        if(doExplosion && !isOneLoopDone)
        {
            StartCoroutine(ExplosionCoroutine());
        }

    }

    void CalculateExpansion()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < maxRangeExplosion && !x.onFire)
            {
                if (distance <= midRange && distance > closeRange) //if it's between close and mid range then it's flammability increases
                {
                    //Debug.Log("medium range");
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    //x.onFire = true;
                    x.IncrementHeat(60);
                    nearObjectsOnFire.Remove(x);
                    secondObjects.Add(x);
                    //break;
                }
                if (distance <= highRange && distance > midRange) //if it's between mid and far range then it's flammability increases
                {
                    //Debug.Log("far range");
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    //x.onFire = true;
                    x.IncrementHeat(30);
                    nearObjectsOnFire.Remove(x);
                    farestObjects.Add(x);
                    //break;
                }
                if (distance <= closeRange) //if it's too close you get on fire instant
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    closestObjects.Add(x);
                    //break;
                }
            }
        }
    }  
    
    public IEnumerator ExplosionKnockBackCor() //Apply force in x sphere radius
    {
        colliders = Physics.OverlapSphere(transform.position, knockbackRadius, explosionMask);
        foreach (Collider target in colliders)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockbackRadius);

        }

        playerCollider = Physics.OverlapSphere(transform.position, knockbackRadius, playerMask);
        foreach (Collider target in playerCollider)
        {
            Rigidbody rb = target.GetComponentInParent<Rigidbody>();
            if (rb == null) continue;
            rb.AddExplosionForce(explosionForce, transform.position, knockbackRadius);

        }

        yield return null;
    }

    public IEnumerator ExplosionCoroutine()
    {
        Debug.Log("Preexplosion!");
        preExplosion = true;
        yield return new WaitForSeconds(2f);
        transform.GetChild(1).gameObject.SetActive(true);
        doExplote = true;
        StartCoroutine(ExplosionKnockBackCor());
        camController.shakeDuration = 0.5f;
        yield return new WaitForSeconds(0.1f);
        isOneLoopDone = true;
        doExplosion = false;
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

        Handles.color = Color.green;
        Handles.DrawSolidDisc(linkedObject.transform.position, Vector3.up, linkedObject.closeRange);
    }
}
#endif
