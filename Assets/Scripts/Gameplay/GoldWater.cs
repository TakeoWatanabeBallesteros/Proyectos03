using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using UnityEngine.Serialization;

public class GoldWater : MonoBehaviour
{
    [Header("Parametros")]
    public float maxWaterDistance;
    [SerializeField] private float distance;
    public LayerMask hitLayer;
    
    [FormerlySerializedAs("collider")]
    [FormerlySerializedAs("cylinderOrigin")]
    [FormerlySerializedAs("cilinderOrigin")]
    [Space(5f)]
    [Header("Componentes")]
    [SerializeField] private CapsuleCollider cylinder;
    [SerializeField] private ParticleSystem water;
    [SerializeField] private ParticleSystem waterDetails;
    private PlayerControls controls;
    public Animator playerAnim;

    [Space(5f)] [Header("Raycast Origins")] 
    [SerializeField] private List<Transform> raysOrigins;

    [Space(5f)] 
    [Header("Sounds")] 
    [SerializeField] private GameObject waterSound;
    
    [SerializeField] private List<FireBehavior> fires;
    private PointsBehavior pointsBehavior;

    [SerializeField] private float currentWater;
    public float maxWater;
    public float waterPerSecond;
    private bool isShooting;

    private Blackboard_UIManager blackboardUIManager;

    private void OnDisable()
    {
        controls.Player.Shoot.started -= Shoot;
        controls.Player.Shoot.canceled -= StopShoot;
    }

    private void Awake()
    {
        controls = controls ?? new PlayerControls();
        controls.Player.Shoot.started += Shoot;
        controls.Player.Shoot.canceled += StopShoot;
        controls.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        fires = new List<FireBehavior>();
        cylinder.enabled = false;
        water.Stop();
        pointsBehavior = Singleton.Instance.PointsManager;
        blackboardUIManager = Singleton.Instance.UIManager.blackboard_UIManager;
        currentWater = maxWater;
        blackboardUIManager.SetWaterBar(currentWater);
    }

    // Update is called once per frame
    void Update()
    {
        distance = ObjectiveDistance();
        SetParticleLength();
        SetColliderScale();
        if(fires.Any()) PuttingOutFires();
        if (currentWater == 0)
        {
            StopShoot();
        }
        ConsumeWater();
    }

    public float GetCurrentWater()
    {
        return currentWater;
    }

    private void ConsumeWater()
    {
        if (currentWater == 0 || !isShooting) return;
        currentWater = Mathf.Clamp( currentWater -= waterPerSecond * Time.deltaTime, 0, maxWater);
        blackboardUIManager.SetWaterBar(currentWater);
    }

    public void Recharge(float waterAmount)
    {
        currentWater = Mathf.Clamp(currentWater += waterAmount, 0, maxWater);
        blackboardUIManager.SetWaterBar(currentWater);
    }

    private void SetParticleLength()
    {
        var weakMain = water.main;
        weakMain.startLifetime = distance / weakMain.startSpeed.constant;
        var weakDetails = waterDetails.main;
        weakDetails.startLifetime = distance / weakDetails.startSpeed.constant;

    }
    private void SetColliderScale()
    {
        cylinder.height = distance;
        cylinder.center = new Vector3(cylinder.center.x, distance/2, cylinder.center.z);
    }
    private float ObjectiveDistance()
    {
        float max = 0;
        foreach (var t in raysOrigins)
        {
            float distance = CastRay(t);
            if (this.distance > max) max = distance;
        }
        return max;
    }
    private float CastRay(Transform origin)
    {
        float rayDistance = 0;
        if (Physics.Raycast(new Ray(origin.position, origin.forward), 
            out RaycastHit hit, maxWaterDistance, hitLayer)) 
        {
            Debug.DrawRay(origin.position, origin.forward * rayDistance);
            return (hit.point - origin.position).magnitude;
        }
        Debug.DrawRay(origin.position, origin.forward * rayDistance);
        return maxWaterDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireSource"))
        {
            fires.Add(other.GetComponentInParent<FireBehavior>());
        }
        else if (other.CompareTag("Collectable"))
        {
            other.GetComponentInParent<Collectable>().TakeDamage(75);
        }
        else if (other.CompareTag("Explosive"))
        {
            other.GetComponent<ExplosionBehavior>().MakeItExplote();
            //StartCoroutine(other.GetComponent<ExplosionBehavior>().Explode());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FireSource"))
        {
            fires.Remove(other.GetComponentInParent<FireBehavior>());    
            other.GetComponentInParent<FireBehavior>().particlesPuttingOut.SetActive(false);
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (currentWater == 0)return;
        playerAnim.SetBool("Shoot",true);
        StartCoroutine(ShootWater());
    }

    private IEnumerator ShootWater()
    {
        yield return new WaitForSeconds(.3f);
        water.Play();
        waterDetails.Play();
        //waterCone.gameObject.SetActive(true);
        cylinder.enabled = true;
        waterSound.SetActive(true);
        isShooting = true;
    }
    
    private void StopShoot(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        StopAllCoroutines();
        water.Stop();
        waterDetails.Stop();
        cylinder.enabled = false;
        fires.Clear();
        pointsBehavior.ResetCombo();
        waterSound.SetActive(false);
        isShooting = false;
        playerAnim.SetBool("Shoot",false);
    }

    private void PuttingOutFires()
    {
        List<FireBehavior> clone = new List<FireBehavior>(fires);
        foreach (var fire in clone)
        {
            fire.PuttingOut();
            if (!fire.onFire)
            {
                fires.Remove(fire);
                fire.enabled = false;
                // Valiendo
            }
        }
    }
}
