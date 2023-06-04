using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirePropagation2 : MonoBehaviour
{
    public List<FirePropagation2> nearObjectsOnFire;
    public List<ExplosionBehavior> nearFiresExplosion;

    public GameObject fire;
    public GameObject explosive;
    public float nearDistance;

    [SerializeField] float fireHP;
    [SerializeField] public float timeToBurn;
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

    public bool startRoomFires;

    bool canLerpMaterials;
    Material _objectMaterial;
    public Material redMaterial;
    public Material burnedMaterial;

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

        nearFiresExplosion = FindObjectsOfType<ExplosionBehavior>().ToList();

        expansionTimer = delay;
        DamageTimer = delayFire;
        fireHP = 100f;

        canLerpMaterials = false;
        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;
        _objectMaterial.EnableKeyword("_EMISSION");

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
                transform.GetChild(1).gameObject.SetActive(true);
                PointsBehavior.AddPointsFire();
                PointsBehavior.IncreaseCombo();
                this.enabled = false;
            }

            if (nearFiresExplosion.Count >= 1)
            {
                CalculateExplosiveFire();
            }

            _objectMaterial.Lerp(_objectMaterial, burnedMaterial, Time.deltaTime / 2);
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
            StartCoroutine(x.Explode());
            nearFiresExplosion.Remove(x);
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
                    fire.GetComponent<FirePropagation2>().canLerpMaterials = true;
                    fire.GetComponent<FirePropagation2>().MaterialLerping(fire.GetComponent<FirePropagation2>().timeToBurn);
                    //fire.GetComponent<MaterialLerping>().canLerpMaterials = true;
                    //nearObjectsOnFire.Remove(x);
                    //break;
                }
                else if (x.fireType == FireType2.LowFlammability && x.timeToBurn >= 0f)
                {
                    fire = x.gameObject;
                    //x.transform.GetChild(0).gameObject.SetActive(true);
                    fire.GetComponent<FirePropagation2>().timeToBurn -= Time.deltaTime;
                    fire.GetComponent<FirePropagation2>().canLerpMaterials = true;
                    fire.GetComponent<FirePropagation2>().MaterialLerping(fire.GetComponent<FirePropagation2>().timeToBurn);
                    //fire.GetComponent<MaterialLerping>().canLerpMaterials = true;
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

    public void MaterialLerping(float timeBurning)
    {
        if (canLerpMaterials)
        {
            _objectMaterial.Lerp(_objectMaterial, redMaterial, Time.deltaTime / timeBurning);
        }
    }
}

public enum FireType2
{
    Explosive,
    HighFlammability,
    LowFlammability
}
