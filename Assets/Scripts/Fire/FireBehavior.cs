using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FireBehavior : MonoBehaviour
{
    public bool onFire;
    [SerializeField] private bool onHeating;

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

    private static readonly int Heat = Shader.PropertyToID("_Heat");
    private static readonly int EmissiveColor = Shader.PropertyToID("_EmissiveColor");

    [SerializeField] private List<Texture> burnSprites;
    [SerializeField] private GameObject decalPrefab;

    private PointsBehavior pointsBehavior;

    // Start is called before the first frame update
    private void Start()
    {
        fireHP = 100f;

        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;

        transform.GetChild(0).gameObject.SetActive(onFire);

        originalFireSize = fireParticles[0].gameObject.transform.localScale.x;
        
        heat = 0;
        if (onFire)
        {
            heat = 100;
            CreateBurnDecal();
        }

        pointsBehavior = Singleton.Instance.PointsManager;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!onFire) CoolDown();

        else
        {
            ApplyHeat();
            ApplyDamage();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Fire":
                var fire = other.GetComponent<FireBehavior>();
                if(fire.onFire) break;
                nearObjects.Add(fire);
                break;
            case "Explosive":
                if (onFire) StartCoroutine(other.GetComponent<ExplosionBehavior>().Explode());
                break;
            case "Player":
                playerHealth = other.GetComponent<PlayerHealth>();
                break;
            case "Kid":
                if(other.GetComponent<ChildrenHealthSystem>()) childrens.Add(other.GetComponent<ChildrenHealthSystem>());
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
                fire.onHeating = false;
                break;
            case "Player":
                playerHealth.isTakingDamage = false;
               // playerHealth = null;
                break;
            case "Kid":
                if (other.GetComponent<ChildrenHealthSystem>())
                { 
                    var children = other.GetComponent<ChildrenHealthSystem>();
                    children.StopBeingBurned();
                    childrens.Remove(children);
                }
                break;
        }
    }

    public void PuttingOut()
    {
        fireHP = Mathf.Clamp(fireHP -= timeToPutOut * Time.deltaTime, 0, 100);
        
        foreach (ParticleSystem fireParticle in fireParticles)
        {
            var scale = (fireHP / 100) * originalFireSize;
            fireParticle.transform.localScale = new Vector3(scale, scale, scale);
        }

        if (fireHP > 0) return;

        pointsBehavior.AddPointsCombo();
        pointsBehavior.AddCombo();
        
        onFire = false;
        
        transform.GetChild(0).gameObject.SetActive(false); //Disable fire
        transform.GetChild(1).gameObject.SetActive(true); //Enable smoke
        SetBurnedMaterial();
        StopHeating();
    }

    public void AddHeat(float heat = 0)
    {
        onHeating = true;
        this.heat = heat == 0 ? Mathf.Clamp(this.heat + (heatPerSecond * Time.deltaTime), 0, 100) : Mathf.Clamp( this.heat += heat, 0, 100);
        _objectMaterial.SetFloat(Heat, Scale(0, 100, this.heat));
        if (this.heat < 100) return;
        onFire = true;
        transform.GetChild(0).gameObject.SetActive(true);
        CreateBurnDecal();
    }

    private void CreateBurnDecal()
    {
        var position = new Vector3(transform.position.x,0,transform.position.z);
        var decal = Instantiate(decalPrefab, position, Quaternion.Euler(Vector3.up)); 
        decal.GetComponentInChildren<MeshRenderer>().material.SetTexture("_BaseMap", burnSprites[Random.Range(0, burnSprites.Count)]);
    }

    private void CoolDown()
    {
        if (onHeating || heat == 0) return;
        heat = Mathf.Clamp(heat - (heatPerSecond * Time.deltaTime), 0, 100);
        _objectMaterial.SetFloat(Heat, Scale(0, 100, heat));
    }

    private void ApplyHeat()
    {
        if(!nearObjects.Any()) return;

        List<FireBehavior> clone = new List<FireBehavior>(nearObjects);

        foreach (var fire in clone)
        {
            var position = transform.position;
            var firePosition = fire.transform.position;
            position.y = 0;
            firePosition.y = 0;
            if(Vector3.Distance(position, firePosition) > heatDistance)
            {
                fire.onHeating = false;
                continue;
            }
            if(fire.heat >= 100 || fire.onFire)
            {
                nearObjects.Remove(fire);
                fire.onHeating = false;
                continue;
            }
            fire.AddHeat();
        }
    }

    private void StopHeating()
    {
        if(!nearObjects.Any()) return;

        List<FireBehavior> clone = new List<FireBehavior>(nearObjects);

        foreach (var fire in clone)
        {
            fire.onHeating = false;
        }
    }

    private void ApplyDamage()
    {
        if (playerHealth != null && Vector3.Distance(playerHealth.transform.position, transform.position) <= damageRadius)
        {
            playerHealth.TakeDamage();
        }
        if(!childrens.Any()) return;
        foreach (var children in childrens)
        {
            if (!children.GetComponent<ChildrenHealthSystem>()) return;
                
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
    
    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, damageRadius);
        Gizmos.color = new Color(1, 0.97f, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, heatDistance);
    }
}

public enum FireType
{
    Explosive,
    HighFlammability,
    LowFlammability
}
