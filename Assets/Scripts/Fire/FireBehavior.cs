using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class FireBehavior : MonoBehaviour
{
    public bool onFire { get; private set; }
    public bool onHeating { get; private set; }
    public bool isWet { get; private set; }

    private List<FireBehavior> nearObjects = new List<FireBehavior>();

    [SerializeField] private GameObject fire;
    
    [SerializeField] float fireHP;

    [SerializeField] private float heatDistance;
    [SerializeField] private float heat; // When heat is over 100 the GameObject will burn
    [SerializeField] private float heatPerSecond = 33.3f; // heat increase per second (yourself)
    // Ex: To reach 100 heat in 1s your heatPerSecond should be 100;

    private ParticleSystem[] fireParticles;
    private float OriginalFireSize;

    Material _objectMaterial;
    public Material redMaterial;
    public Material burnedMaterial;
    
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        fireHP = 100f;
        heat = 0;
        isWet = false;

        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;
        _objectMaterial.EnableKeyword("_EMISSION");

        transform.GetChild(0).gameObject.SetActive(onFire);

        fireParticles = GetComponentsInChildren<ParticleSystem>();
        OriginalFireSize = fireParticles[0].gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // if (onFire)
        // {
        //     if (fireHP <= 0)
        //     {
        //         transform.GetChild(0).gameObject.SetActive(false);
        //         StopAllCoroutines();
        //         transform.GetChild(1).gameObject.SetActive(true);
        //         PointsBehavior.AddPointsFire();
        //         PointsBehavior.IncreaseCombo();
        //         this.enabled = false;
        //     }
// 
        //     _objectMaterial.Lerp(_objectMaterial, burnedMaterial, Time.deltaTime / 2);
        // }

        if(!nearObjects.Any()) ApplyHeat();
        if (!onHeating) heat -= heatPerSecond * Time.deltaTime;
    }
    
    // TODO: Set player and kid health condition
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Fire":
                var fire = other.GetComponent<FireBehavior>();
                if(fire.onFire)
                    break;
                fire.onHeating = true;
                nearObjects.Add(fire);
                break;
            case "Explosive":
                StartCoroutine(other.GetComponent<ExplosionBehavior>().Explode());
                break;
            case "Player":
                // Set Player health burning
                break;
            case "Kid":
                // Set kid burning
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Fire":
                nearObjects.Remove(other.GetComponent<FireBehavior>());
                break;
            case "Player":
                // Set Player health not burning
                break;
            case "Kid":
                // Set kid not burning
                break;
        }
    }

    public void PuttingOut(float damage)
    {
        // Damage
        
        foreach (ParticleSystem fireParticle in fireParticles)
        {
            float scale = (fireHP / 100) * OriginalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void AddHeat()
    {
        heat += heatPerSecond * Time.deltaTime; 
    }
    public void AddHeat(float heat)
    {
        this.heat += heat;
    }

    private void ApplyHeat()
    {
        foreach (var fire in nearObjects)
        {
            fire.AddHeat();
            if(fire.heat >= 100) nearObjects.Remove(fire);
        }
    }

    // Check third param
    public void MaterialLerp(float timeBurning) => _objectMaterial.Lerp(_objectMaterial, redMaterial, Time.deltaTime / timeBurning);
}

public enum FireType
{
    Explosive,
    HighFlammability,
    LowFlammability
}
