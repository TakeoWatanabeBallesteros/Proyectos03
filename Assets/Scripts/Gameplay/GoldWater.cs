using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class GoldWater : MonoBehaviour
{
    [Header("Parametros")]
    public float maxWaterDistance;
    [SerializeField] float distance;
    public LayerMask HitLayer;
    public float ScaleFactor;
    
    [Space(5f)]
    [Header("Componentes")]
    [SerializeField] Transform cilinderOrigin;
    [SerializeField] ParticleSystem water;
    [SerializeField] ParticleSystem waterCone;
    [SerializeField] PlayerControls Controls;
    public GameObject colider;

    [Space(5f)] [Header("Raycast Origins")] 
    [SerializeField] private List<Transform> raysOrigins;

    [Space(5f)] 
    [Header("Sounds")] 
    [SerializeField] private GameObject waterSound;
    
    [SerializeField] List<FireBehavior> Fires;
    private PointsBehavior pointsBehavior;
    
    private void Awake()
    {
        Controls = Controls ?? new PlayerControls();
        Controls.Player.Shoot.started += Shoot;
        Controls.Player.Shoot.canceled += StopShoot;
        Controls.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        Fires = new List<FireBehavior>();
        colider.SetActive(false);
        water.Stop();
        pointsBehavior = Singleton.Instance.PointsManager;
    }

    // Update is called once per frame
    void Update()
    {
        distance = ObjectiveDistance();
        SetParticleLength();
        SetColliderScale();
        if(Fires.Any()) PuttingOutFires();
    }

    private void SetParticleLength()
    {
        var WeakMain = water.main;
        WeakMain.startLifetime = distance / WeakMain.startSpeed.constant;
    }
    private void SetColliderScale()
    {
        cilinderOrigin.localScale = new Vector3(1,1, distance/ScaleFactor);
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
            out RaycastHit hit, maxWaterDistance, HitLayer)) 
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
            Fires.Add(other.GetComponentInParent<FireBehavior>());
        }
        else if (other.CompareTag("Collectable"))
        {
            other.GetComponentInParent<Collectable>().TakeDamage(75);
            if (other.GetComponentInParent<Collectable>().Destroyed) return;
            pointsBehavior.RemovePointsCollectable();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FireSource"))
        {
            Fires.Remove(other.GetComponentInParent<FireBehavior>());
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        water.Play();
        waterCone.gameObject.SetActive(true);
        colider.SetActive(true);
        waterSound.SetActive(true);
    }
    private void StopShoot(InputAction.CallbackContext context)
    {
        water.Stop();
        waterCone.gameObject.SetActive(false);
        colider.SetActive(false);
        Fires.Clear();
        pointsBehavior.ResetCombo();
        waterSound.SetActive(false);
    }

    private void PuttingOutFires()
    {
        List<FireBehavior> clone = new List<FireBehavior>(Fires);
        foreach (var fire in clone)
        {
            fire.PuttingOut();
            if (!fire.onFire)
            {
                Fires.Remove(fire);
                fire.enabled = false;
                // Valiendo
            }
        }
    }
}
