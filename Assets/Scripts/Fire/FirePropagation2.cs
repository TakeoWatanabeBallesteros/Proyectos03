using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirePropagation2 : MonoBehaviour
{
    public List<FirePropagation2> nearObjectsOnFire;
    public List<ObjectsExplosionv2> nearFiresExplosion;

    public GameObject fire;
    public GameObject explosive;
    public float nearDistance;

    public int highPercentage;
    public int lowPercentage;

    [SerializeField] float fireHP;
    [SerializeField] float timeToBurn;
    float timeToExplote;

    public bool onFire = false;

    public FireType2 fireType;

    float expansionTimer;
    public float delay;

    float DamageTimer;
    public float delayFire;

    public bool CanBurn;

    [SerializeField] ParticleSystem[] fireParticles;
    private float OriginalFireSize;

    CameraController camController;

    public Material objMaterial;
    public Material redMaterial;

    public bool startRoomFires;

    private void Awake()
    {
        fireParticles = GetComponentsInChildren<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CanBurn = true;
        startRoomFires = false;

        nearObjectsOnFire = FindObjectsOfType<FirePropagation2>().ToList();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);

        nearFiresExplosion = FindObjectsOfType<ObjectsExplosionv2>().ToList();
        nearFiresExplosion.RemoveAll(item => item.doExplote == true);

        expansionTimer = delay;
        DamageTimer = delayFire;
        fireHP = 100f;

        if (!onFire)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (fireType == FireType2.LowFlammability)
        {
            timeToBurn = 10f;
        }

        else if (fireType == FireType2.HighFlammability)
        {
            timeToBurn = 5f;
        }

        OriginalFireSize = fireParticles[0].gameObject.transform.localScale.x;
        camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (onFire)
        {
            CalculateFirePropagation();

            if (fireHP > 0 && DamageTimer > 0)
            {
                DamageTimer -= Time.deltaTime;
            }

            if (fireHP <= 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(SmokeWork());
            }

            if (nearFiresExplosion.Count >= 1)
            {
                CalculateExplosiveFire();
            }
        }

        if (timeToBurn <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            onFire = true;
            CanBurn = false;
        }

        foreach (ParticleSystem fireParticle in fireParticles)
        {
            float scale = fireHP / 100 * OriginalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }

    }

    public void CalculateExplosiveFire()
    {
        foreach (var x in nearFiresExplosion)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);
            //nearFiresExplosion.RemoveAll(x => !x);

            if (distance < nearDistance && !x.preExplosion)
            {
                if (x.isOneLoopDone == false)
                {
                    explosive = x.gameObject;
                    StartCoroutine(ExplosionThings());
                    x.enabled = false;
                    nearFiresExplosion.Remove(x);                    
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

    public void IncrementHeat(float heat)
    {        
        timeToBurn -= (timeToBurn * (heat/100));
    }

    void CalculateFirePropagation()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && !x.onFire)
            {
                if (x.fireType == FireType2.HighFlammability && x.timeToBurn >= 0f)
                {
                    fire = x.gameObject;
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    fire.GetComponent<FirePropagation2>().timeToBurn -= Time.deltaTime;
                    fire.GetComponent<MaterialLerping>().canLerpMaterials = true;
                    //nearObjectsOnFire.Remove(x);
                    //break;
                }
                else if (x.fireType == FireType2.LowFlammability && x.fireHP >= 0f)
                {
                    fire = x.gameObject;
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    fire.GetComponent<FirePropagation2>().timeToBurn -= Time.deltaTime;
                    fire.GetComponent<MaterialLerping>().canLerpMaterials = true;
                    //nearObjectsOnFire.Remove(x);
                    //break;
                }
                else if (x.fireHP <= 0)
                {
                    CheckStartingFire(x.gameObject);
                }
            }
        }
    }

    void CheckStartingFire(GameObject fire)
    {
        if (fire != null)
        {
            fire.transform.GetChild(0).gameObject.SetActive(true);
            fire.GetComponent<FirePropagation2>().onFire = true;
        }
    }

    public IEnumerator ExplosionThings()
    {
        Debug.Log("Preexplosion!");
        explosive.GetComponent<ObjectsExplosionv2>().preExplosion = true;
        yield return new WaitForSeconds(2f);
        explosive.transform.GetChild(1).gameObject.SetActive(true);
        explosive.GetComponent<ObjectsExplosionv2>().doExplote = true;
        StartCoroutine(explosive.GetComponent<ObjectsExplosionv2>().ExplosionKnockBackCor());
        camController.shakeDuration = 1f;
        yield return new WaitForSeconds(1.5f);
        
    }

    IEnumerator SmokeWork()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
    }
}

public enum FireType2
{
    Explosive,
    HighFlammability,
    LowFlammability
}
