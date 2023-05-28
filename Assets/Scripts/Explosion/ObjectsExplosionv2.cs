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
    public List<FirePropagation> nearObjectsOnFire = new List<FirePropagation>();
    public List<FirePropagation> closestObjects = new List<FirePropagation>();
    public List<FirePropagation> secondObjects = new List<FirePropagation>();
    public List<FirePropagation> farestObjects = new List<FirePropagation>();

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
    public LayerMask explosionMask;

    public bool isOneLoopDone = false;

    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagation>().ToList<FirePropagation>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        expansionExplosionTimer = delayExplosionTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOneLoopDone == false)
        {
            if (preExplosion == true)
            {
                animator.SetTrigger("Explote");
            }

            if (doExplote == true && expansionExplosionTimer >= 0f)
            {
                Debug.Log("Expansion on explosion");
                preExplosion = false;
                expansionExplosionTimer -= Time.deltaTime;

                if (expansionTimer >= 0f)
                {
                    expansionTimer -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Expansion");
                    expansionTimer = delay;
                    CalculateExpansion();
                }
            }
            else
            {
                expansionExplosionTimer = delayExplosionTimer;
                doExplote = false;
            }

            isOneLoopDone = true;
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
                    nearObjectsOnFire.Remove(x);
                    secondObjects.Add(x);
                    //break;
                }
                else if (distance <= highRange && distance > midRange) //if it's between mid and far range then it's flammability increases
                {
                    //Debug.Log("far range");
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    //x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    farestObjects.Add(x);
                    //break;
                }
                else if (distance <= closeRange) //if it's too close you get on fire instant
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
        yield return null;
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
