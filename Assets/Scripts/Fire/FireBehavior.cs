using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FireBehavior : MonoBehaviour
{
    public bool onFire;
    public bool onHeating { get; private set; }
    public bool isWet { get; private set; }

    private List<FireBehavior> nearObjects = new List<FireBehavior>();
    private List<ChildrenHealthSystem> childrens = new List<ChildrenHealthSystem>();
    private PlayerHealth playerHealth;
    [SerializeField] private float damageRadius;

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
    

    private int objectsHeatingCount = 0;
    private static readonly int Heat = Shader.PropertyToID("_Heat");

    private PointsBehavior pointsBehavior;

    // Start is called before the first frame update
    void Start()
    {
        fireHP = 100f;
        isWet = false;

        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;

        transform.GetChild(0).gameObject.SetActive(onFire);

        originalFireSize = fireParticles[0].gameObject.transform.localScale.x;
        
        heat = 0;
        if(onFire) AddHeat(100);
        // pointsBehavior = Singleton.Instance.PointsManager;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyHeat();

        ApplyDamage();
        
        CoolDown();
    }
    
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
                playerHealth = other.GetComponent<PlayerHealth>();
                break;
            case "Kid":
                childrens.Add(other.GetComponent<ChildrenHealthSystem>());
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
                playerHealth.isTakingDamage = false;
                playerHealth = null;
                break;
            case "Kid":
                var children = other.GetComponent<ChildrenHealthSystem>();
                children.StopBeingBurned();
                childrens.Remove(children);
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

        if (fireHP > 0) return;
        
        transform.GetChild(0).gameObject.SetActive(false);
        StopAllCoroutines();
        transform.GetChild(1).gameObject.SetActive(true);
        // pointsBehavior.AddPointsCombo();
        // pointsBehavior.AddCombo();
        enabled = false;
    }

    private void AddHeat()
    {
        heat += heatPerSecond * Time.deltaTime;
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
        if (!(heat > 100)) return;
        onFire = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void AddHeat(float heat)
    {
        this.heat += heat;
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
        if (this.heat < 100) return;
        onFire = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void CoolDown() 
    { 
        if (!onFire && !onHeating && heat > 0) heat -= heatPerSecond * Time.deltaTime;
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
    }

    private void ApplyHeat()
    {
        if(!nearObjects.Any()) return;

        List<FireBehavior> clone = new List<FireBehavior>(nearObjects);

        foreach (var fire in clone)
        {
            if(Vector3.Distance(fire.transform.position, transform.position) > heatDistance) continue;
            if(fire.heat >= 100 || fire.onFire) nearObjects.Remove(fire);
            fire.AddHeat();
        }
    }

    private void ApplyDamage()
    {
        if (playerHealth != null && Vector3.Distance(playerHealth.transform.position, transform.position) < damageRadius)
        {
            playerHealth.TakeDamage();
        }
        if(!childrens.Any()) return;
        foreach (var children in childrens)
        {
            if (Vector3.Distance(children.transform.position, transform.position) > damageRadius) continue;
            children.TakeDamage();
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
