using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    [SerializeField] private float timeToPutOut;

    [SerializeField] private float heatDistance;
    [SerializeField]
    [Range(0, 100)]
    private float heat; // When heat is over 100 the GameObject will burn
    [SerializeField] private float heatPerSecond = 33.3f; // heat increase per second (yourself)
    // Ex: To reach 100 heat in 1s your heatPerSecond should be 100;

    [SerializeField] private List<ParticleSystem> fireParticles;
    private float originalFireSize;

    Material _objectMaterial;
    public Material burnedMaterial;
    
    private PlayerHealth playerHealth;

    private int objectsHeatingCount = 0;
    private static readonly int Heat = Shader.PropertyToID("_Heat");

    private PointsBehavior pointsBehavior;

    // Start is called before the first frame update
    void Start()
    {
        fireHP = 100f;
        heat = 0;
        isWet = false;

        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;

        transform.GetChild(0).gameObject.SetActive(onFire);

        originalFireSize = fireParticles[0].gameObject.transform.localScale.x;
        
        pointsBehavior = Singleton.Instance.PointsManager;
    }

    // Update is called once per frame
    void Update()
    {
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
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
        // if (!onHeating && heat > 0) heat -= heatPerSecond * Time.deltaTime;
    }
    
    // TODO: Set player health condition
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
                other.GetComponent<ChildrenHealthSystem>().TakeDamage();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Fire":
                var fire = other.GetComponent<FireBehavior>();
                nearObjects.Remove(fire);
                fire.objectsHeatingCount--;
                fire.onHeating = fire.objectsHeatingCount == 0;
                break;
            case "Player":
                // Set Player health not burning
                break;
            case "Kid":
                // Set kid not burning
                other.GetComponent<ChildrenHealthSystem>().StopBeingBurned();
                break;
        }
    }

    public void PuttingOut()
    {
        // Damage
        fireHP -= timeToPutOut * Time.deltaTime;
        
        foreach (ParticleSystem fireParticle in fireParticles)
        {
            float scale = (fireHP / 100) * originalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }

        if (!(fireHP <= 0)) return;
        transform.GetChild(0).gameObject.SetActive(false);
        StopAllCoroutines();
        transform.GetChild(1).gameObject.SetActive(true);
        pointsBehavior.AddPointsCombo();
        pointsBehavior.AddCombo();
            
        this.enabled = false;
    }

    private void AddHeat()
    {
        heat += heatPerSecond * Time.deltaTime; 
    }
    public void AddHeat(float heat)
    {
        this.heat += heat;
        if (this.heat >= 100) transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ApplyHeat()
    {
        foreach (var fire in nearObjects)
        {
            fire.AddHeat();
            if(fire.heat >= 100) nearObjects.Remove(fire);
        }
    }


    private static float Scale(float min, float max, float value)
    {
        // max - min / bla bla bla + max
        return Mathf.Clamp(2 / (max - min) * (value - max) + 3, 1, 3);
    }
}

public enum FireType
{
    Explosive,
    HighFlammability,
    LowFlammability
}
