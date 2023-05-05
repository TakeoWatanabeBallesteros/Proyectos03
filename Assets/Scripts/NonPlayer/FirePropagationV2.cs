using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirePropagationV2 : MonoBehaviour
{
    public List<FirePropagationV2> allFires;
    public List<GameObject> nearFiresExplosion;

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
    // Start is called before the first frame update
    void Start()
    {
        CanBurn = true;
        allFires = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        allFires.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        DamageTimer = delayFire;
        if (!onFire)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        fireParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
        
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
            fireParticle.transform.localScale = new Vector3(fireHP / 100, fireHP / 100, fireHP / 100);
        }
    }

    public void CalculateFireProp()
    {     
        foreach (var x in allFires)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && !x.onFire)
            {
                if (x.fireType == FireType.HighFlammability && Random.Range(1,101) < highPercentage)
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    allFires.Remove(x);
                    break;
                }
                else if(x.fireType == FireType.LowFlammability && Random.Range(1, 101) < lowPercentage)
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    allFires.Remove(x);
                    break;
                }
                else if (x.fireType == FireType.Explosive)
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);

                    StartCoroutine(WaitToStartFire());
                    x.onFire = true;
                    allFires.Remove(x);
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

    IEnumerator WaitToStartFire()
    {
        yield return new WaitForSeconds(2f);
        fire.SetActive(true);
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
