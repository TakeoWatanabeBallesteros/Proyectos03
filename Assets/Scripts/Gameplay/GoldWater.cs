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

    [Space(5f)]
    [Header("Raycast Origins")]
    public Transform R0;
    public Transform R1;
    public Transform R2;
    public Transform R3;
    public Transform R4;

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
        float[] Raylengths = new float[5];
        Raylengths[0] = CastRay(R0);
        Raylengths[1] = CastRay(R1);
        Raylengths[2] = CastRay(R2);
        Raylengths[3] = CastRay(R3);
        Raylengths[4] = CastRay(R4);
        float max = Raylengths[0];
        for (int i = 0; i < Raylengths.Length; i++)
        {
            if (Raylengths[i] > max)
            {
                max = Raylengths[i];
            }
        }
        return max;
    }
    private float CastRay(Transform Origin)
    {
        Ray ray = new Ray(Origin.position, Origin.forward);
        float Rdist = 0;
        if (Physics.Raycast(ray, out RaycastHit hit, maxWaterDistance, HitLayer))
        {
            Rdist = (hit.point - Origin.position).magnitude;
        }
        else
        {
            Rdist = maxWaterDistance;
        }
        Debug.DrawRay(Origin.position, Origin.forward * Rdist);
        return Rdist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireSource"))
        {
            Fires.Add(other.GetComponentInParent<FireBehavior>());
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
        foreach (var fire in Fires)
        {
            fire.PuttingOut();
        }
    }
}
