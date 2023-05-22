using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirePropagation2 : MonoBehaviour
{
    public List<FirePropagation2> nearObjectsOnFire;
    public List<FirePropagation2> nearFiresExplosion;

    public GameObject fire;
    public float nearDistance;

    public int highPercentage;
    public int lowPercentage;

    [SerializeField] float fireHP;
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

    CameraController camController;
    // Start is called before the first frame update
    void Start()
    {
        CanBurn = true;
        nearObjectsOnFire = FindObjectsOfType<FirePropagation2>().ToList();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        DamageTimer = delayFire;

        if (!onFire)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (fireType == FireType.LowFlammability)
        {
            fireHP = 100f;
        }
        
        else if (fireType == FireType.HighFlammability)
        {
            fireHP = 60f;
        }       
        

        OriginalFireSize = fireParticles[0].gameObject.transform.localScale.x;
        camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (onFire)
        {            
            if (fireHP <= 0)
            {
                onFire = true;
                CanBurn = false;
                transform.GetChild(0).gameObject.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(SmokeWork());
            }

            CalculateFirePropagation();
            CalculateExplosiveFire();
        }

        foreach (ParticleSystem fireParticle in fireParticles)
        {
            float scale = fireHP / 100 * OriginalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void CalculateExplosiveFire()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && !x.onFire)
            {
                if (x.fireType == FireType.Explosive)
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

    void CalculateFirePropagation()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && !x.onFire)
            {
                if (x.fireType == FireType.HighFlammability && x.fireHP >= 0f)
                {
                    fire = x.gameObject;
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    fire.GetComponent<FirePropagation2>().fireHP -= Time.deltaTime;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if (x.fireType == FireType.LowFlammability && x.fireHP >= 0f)
                {
                    fire = x.gameObject;
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    fire.GetComponent<FirePropagation2>().fireHP -= Time.deltaTime;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
            }
        }
    }

    void ExplosionCalculation()
    {
        StartCoroutine(ExplosionThings());
    }

    IEnumerator ExplosionThings()
    {
        Debug.Log("Preexplosion!");
        fire.GetComponent<ObjectsExplosion>().preExplosion = true;
        yield return new WaitForSeconds(2f);
        fire.transform.GetChild(1).gameObject.SetActive(true);
        fire.GetComponent<ObjectsExplosion>().doExplote = true;
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
