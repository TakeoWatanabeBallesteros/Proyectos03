using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;


public class FirePropagationV2 : MonoBehaviour
{
    public List<FirePropagationV2> nearObjectsOnFire;
    public List<FirePropagationV2> nearFiresExplosion;

    public GameObject fire;
    public float nearDistance;
        
    public int highPercentage;
    public int lowPercentage;

    [SerializeField]float fireHP = 100f;
    float timeToExplote;

    public bool onFire = false;

    public FireType fireType;

    float expansionTimer;
    public float delay;

    float DamageTimer;
    public float delayFire;

    public bool CanBurn;

    [SerializeField] ParticleSystem[] fireParticles;
    private float OriginalFireSize;

    // Start is called before the first frame update
    void Start()
    {
        CanBurn = true;
        nearObjectsOnFire = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        DamageTimer = delayFire;
        if (!onFire)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        fireParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
        OriginalFireSize = fireParticles[0].gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire)
        {
            if (expansionTimer >= 0f)
            {
                expansionTimer -= Time.deltaTime;
            }
            else
            {
                expansionTimer = delay;
                CalculateFireProp();
            }
            if (fireHP <= 0)
            {
                onFire = false;
                CanBurn = false;
                transform.GetChild(0).gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(SmokeWork());
            }

            if (fireHP > 0 && DamageTimer > 0)
            {
                DamageTimer -= Time.deltaTime;
            }
        }

        foreach (ParticleSystem fireParticle in fireParticles)
        {
            float scale = fireHP / 100 * OriginalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void CalculateFireProp()
    {     
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && !x.onFire)
            {
                if (x.fireType == FireType.HighFlammability && Random.Range(1,101) < highPercentage)
                {
                    fire = x.gameObject;
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if(x.fireType == FireType.LowFlammability && Random.Range(1, 101) < lowPercentage)
                {
                    fire = x.gameObject;
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if (x.fireType == FireType.Explosive)
                {
                    fire = x.gameObject;
                    ExplosionCalculation();
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);                     
                    break;
                }
            }
        }          

    }   
    public void TakeDamage(float DMG)
    {
        if (DamageTimer > 0)
        {
            return;
        }
        fireHP -= DMG;
        DamageTimer = delayFire;
    }

    public void ExplosionCalculation()
    {
        StartCoroutine(ExplosionThings());
        //fire.GetComponent<Renderer>().material.Lerp(mat1, mat2, 1f);
    }

    IEnumerator ExplosionThings()
    {
        Debug.Log("Preexplosion!");
        fire.GetComponent<ObjectsExplosion>().preExplosion = true;
        yield return new WaitForSeconds(2f);
        fire.transform.GetChild(1).gameObject.SetActive(true);
        fire.GetComponent<ObjectsExplosion>().doExplote = true;
    }

    IEnumerator SmokeWork()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
    }
    
}

public enum FireType
{    
    Explosive,
    HighFlammability,    
    LowFlammability
}
