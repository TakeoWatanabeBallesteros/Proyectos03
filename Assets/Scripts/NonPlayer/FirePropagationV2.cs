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
    // Start is called before the first frame update
    void Start()
    {
        CanBurn = true;
        //firePrefab = Resources.Load("Prefabs/Firee") as GameObject;
        allFires = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        allFires.RemoveAll(item => item.onFire == true);
        expansionTimer = delay;
        DamageTimer = delayFire;
        if (!onFire)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
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
            }

            if (fireHP > 0 && DamageTimer > 0)
            {
                DamageTimer -= Time.deltaTime;
            }
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
                    //x.transform.GetChild(1).gameObject.SetActive(true);

                    StartCoroutine(WaitToStartFire());
                    x.onFire = true;
                    allFires.Remove(x);
                    break;
                }
            }
        }          

    }   
    public void TakeDamage()
    {
        if (DamageTimer > 0)
        {
            return;
        }
        fireHP -= 25f;
        DamageTimer = delayFire;
    }

    IEnumerator WaitToStartFire()
    {
        yield return new WaitForSeconds(2f);
        fire.SetActive(true);
    }
    
}

public enum FireType
{    
    Explosive,
    HighFlammability,    
    LowFlammability
}
