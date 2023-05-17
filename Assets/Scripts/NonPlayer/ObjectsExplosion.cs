using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

public class ObjectsExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    public bool doExplote = false;
    public bool preExplosion = false;
    public List<FirePropagationV2> nearObjectsOnFire = new List<FirePropagationV2>();
    public List<FirePropagationV2> closestObjects = new List<FirePropagationV2>();
    public List<FirePropagationV2> secondObjects = new List<FirePropagationV2>();
    public List<FirePropagationV2> farestObjects = new List<FirePropagationV2>();

    public float closeRange;
    public float midRange;
    public float highRange;

    public float midRangePercentage;
    public float highRangePercentage;

    float expansionTimer;
    public float delay;
    float expansionExplosionTimer;
    public float delayExplosionTimer;

    public float maxRangeExplosion;

    public Animator animator;

    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        expansionExplosionTimer = delayExplosionTimer;
    }

    // Update is called once per frame
    void Update()
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
        
    }

    void CalculateExpansion()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < maxRangeExplosion && !x.onFire)
            {
                if (distance <= midRange && distance > closeRange && Random.Range(1, 101) < midRangePercentage) //if it's between close and mid range then it's flammability increases
                {
                    Debug.Log("medium range");
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    secondObjects.Add(x);
                    break;
                }
                else if (distance <= highRange && distance > midRange && Random.Range(1, 101) < highRangePercentage) //if it's between mid and far range then it's flammability increases
                {
                    Debug.Log("far range");
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    farestObjects.Add(x);
                    break;
                }
                else if (distance <= closeRange) //if it's too close you get on fire instant
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    closestObjects.Add(x);
                    break;
                }
            }
        }
    }

}

//how to make a solid disc to checj radius from an object 
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
