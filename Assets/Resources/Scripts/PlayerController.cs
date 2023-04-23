using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController p_CharacterController;
    Rigidbody p_Rigidbody;
    Vector3 Knockback;
    Vector3 m_MoveDir;
    //public Transform m_Orientation;

    float m_VerticalSpeed = 0.0f;
    public bool m_OnGround = true;
    public float m_JumpSpeed = 15.0f;
    float distToGround = 1f;

    public Camera m_Camera;
    CameraController m_CameraController;
    public float m_LerRotationPct = 0.85f;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 6.5f;
    public bool m_PlayerMoving = false;

    public float m_MaxShootDistance;
    public LayerMask m_WaterLayerMask;

    bool m_IsJumpEnabled = true;

    float Force;
    public float initialForce = 30f;
    public float maxForce = 200f;
    public float incrementForce = 1.5f;

    float elevateForce;
    public float elevateInitialForce = 4f;
    public float elevateMaxForce = 500f;
    public float elevateIncrementForce;

    Ray l_Ray;
    RaycastHit l_RayCastHit;
    Ray _cameraRay;
    RaycastHit _cameraRayHit;

    float m_VerticalInput;
    float m_HorizontalInput;
    private Vector3 _input;
    float _turnSpeed = 360f;

    private GameObject mainCameraGO;

    [SerializeField]
    private GameObject waterParticles;
    ParticleSystem chorroAgua;
    bool canShootWater = true;


    // Start is called before the first frame update
    void Start()
    {
        p_Rigidbody = GetComponent<Rigidbody>();
        m_CameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        mainCameraGO = GameObject.FindGameObjectWithTag("MainCamera");
        m_Camera = Camera.main;
        Force = initialForce;
        elevateForce = elevateInitialForce;
        chorroAgua = waterParticles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            //KnockBackv2();
            ShootWater();
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            /*
            Force = initialForce;
            elevateForce = elevateInitialForce;
            p_Rigidbody.useGravity = true;
            Physics.gravity = Vector3.Lerp(Physics.gravity, new Vector3(0f, -10f, 0), 2f);*/
            chorroAgua.Stop();
        }


        IsGrounded();
        GatherInput();
        Look();

        if (m_OnGround)
        {
            Move();
        }

        /*
        if (!m_OnGround && Input.GetKey(KeyCode.Space))
        {
            if (Input.GetMouseButton(0))
            {
                KnockBackv2();
            }
        }*/

        if (Input.GetKeyDown(KeyCode.Q))
        {
            mainCameraGO.transform.eulerAngles = Vector3.Lerp(mainCameraGO.transform.eulerAngles, mainCameraGO.transform.eulerAngles +
                new Vector3(0f, 90f, 0f), 3f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            mainCameraGO.transform.Rotate(0f, -90f, 0f, Space.World);
        }

    }


    public void IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f))
            m_OnGround = true;
        else
            m_OnGround = false;
    }
      

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        p_Rigidbody.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * m_WalkSpeed * Time.deltaTime);
    }

    void LookAtMouse()
    {
        _cameraRay = m_Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_cameraRay, out _cameraRayHit))
        {
            if (_cameraRayHit.transform.tag == "Ground")
            {
                Vector3 targetPosition = new Vector3(_cameraRayHit.point.x, transform.position.y, _cameraRayHit.point.z);
                transform.LookAt(targetPosition);
            }
        }
    }

    public void KnockBackv2()
    {               
        Ray l_Ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_RayCastHit;
        //Debug.DrawRay(m_Camera.transform.position, direction * 100, Color.green, 5f);
        p_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        chorroAgua.Play();

        if (Physics.Raycast(l_Ray, out l_RayCastHit, m_MaxShootDistance, m_WaterLayerMask.value))
        {

            if (l_RayCastHit.transform.tag == "CanMove")
            {
                m_MoveDir = (l_RayCastHit.transform.position - transform.position).normalized;
                l_RayCastHit.rigidbody.AddForce(m_MoveDir, ForceMode.Impulse);

            }
            else if (l_RayCastHit.transform.tag == "Fire")
            {
                l_RayCastHit.transform.gameObject.SetActive(false);
            }
            else
            {
                Knockback = (l_RayCastHit.point - transform.position).normalized;
                Force += incrementForce * Time.deltaTime;
                p_Rigidbody.AddForce(-Knockback * Force, ForceMode.Force);
                Force = Mathf.Clamp(Force, 0, maxForce);
                waterParticles.transform.LookAt(l_RayCastHit.point);
                //Debug.Log(Force);
            }
        }
    }

    public void ShootWater()
    {
        Ray l_Ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_RayCastHit;
        chorroAgua.Play();

        if (Physics.Raycast(l_Ray, out l_RayCastHit, m_MaxShootDistance, m_WaterLayerMask.value))
        {
            waterParticles.transform.LookAt(l_RayCastHit.point);
            if (l_RayCastHit.transform.tag == "CanMove")
            {
                m_MoveDir = (l_RayCastHit.transform.position - transform.position).normalized;
                l_RayCastHit.rigidbody.AddForce(m_MoveDir, ForceMode.Impulse);

            }
            if (l_RayCastHit.transform.tag == "Burning")
            {              
                l_RayCastHit.transform.GetChild(0).gameObject.SetActive(false);
            }            
        }
    }

    public void JetpackForce()
    {
        p_Rigidbody.constraints = ~RigidbodyConstraints.FreezePositionY;
        p_Rigidbody.AddForce(Vector3.up * elevateForce, ForceMode.Force);
        p_Rigidbody.useGravity = false;
        //Debug.Log(elevateForce);
        //elevateForce += elevateIncrementForce * Time.deltaTime;
        //elevateForce = Mathf.Clamp(elevateForce, 0, elevateMaxForce);
    }

}
