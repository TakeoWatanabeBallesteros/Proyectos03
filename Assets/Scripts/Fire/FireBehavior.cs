using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = FMOD.Debug;
using Random = UnityEngine.Random;

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
    public float timeToPutOut;

    [SerializeField] private float heatDistance;
    [SerializeField]
    [Range(0, 100)]
    private float heat; // When heat is over 100 the GameObject will burn
    [SerializeField] private float heatPerSecond = 33.3f; // heat increase per second (yourself)
    // Ex: To reach 100 heat in 1s your heatPerSecond should be 100;

    [SerializeField] private List<ParticleSystem> fireParticles;
    private float originalFireSize;

    private Material _objectMaterial;

    private int objectsHeatingCount = 0;
    private static readonly int Heat = Shader.PropertyToID("_Heat");
    private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");

    public List<Texture> burnSprites;
    [SerializeField] private GameObject decalPrefab;

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
        if (onFire)
        {
            AddHeat(100);
            CreateBurnDecal();
        }

        pointsBehavior = Singleton.Instance.PointsManager;
    }

    // Update is called once per frame
    void Update()
    {
        if(!onFire) return;
        
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
                if(fire.onFire) break;
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
        
        transform.GetChild(0).gameObject.SetActive(false); //Disable fire
        StopAllCoroutines();
        transform.GetChild(1).gameObject.SetActive(true); //Enable smoke
        SetBurnedMaterial();
        enabled = false;
        pointsBehavior.AddPointsCombo();
        pointsBehavior.AddCombo();
    }

    private void AddHeat()
    {
        heat = Mathf.Clamp(heat + (heatPerSecond * Time.deltaTime), 0, 100);
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
        if (heat < 100 || onFire) return;
        onFire = true;
        transform.GetChild(0).gameObject.SetActive(true);
        CreateBurnDecal();
    }
    public void AddHeat(float heat)
    {
        heat = Mathf.Clamp( this.heat += heat, 0, 100);
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
        if (this.heat < 100 || onFire) return;
        onFire = true;
        transform.GetChild(0).gameObject.SetActive(true);
        CreateBurnDecal();
    }

    private void CreateBurnDecal()
    {
        UnityEngine.Debug.Log("Isaac");
        var decal = Instantiate(decalPrefab, transform.position, Quaternion.Euler(Vector3.up)); 
        decal.GetComponentInChildren<MeshRenderer>().material.SetTexture("_BaseMap", burnSprites[Random.Range(0, burnSprites.Count)]);
    }

    private void CoolDown()
    {
        if (!onFire && !onHeating && heat > 0) heat = Mathf.Clamp(heat -(heatPerSecond * Time.deltaTime), 0, 100);
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

    private void SetBurnedMaterial()
    {
        _objectMaterial.DisableKeyword("_EMISSION");
        _objectMaterial.SetColor(EmissiveColor, Color.black);
    }
}

public enum FireType
{
    Explosive,
    HighFlammability,
    LowFlammability
}
