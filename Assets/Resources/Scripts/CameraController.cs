using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_LookAtTransform;
    public float m_MaxDistance = 15.0f;
    public float m_MinDistance = 15.0f;
    public float m_YawRotationalSpeed = 360.0f;
    public float m_PitchRotationalSpeed = 180.0f;

    public float m_TimetoResetCamera;
    float resetCameraSave;
    PlayerController m_Player;

    [SerializeField] float m_Pitch = 0.0f;
    public float m_MinPitch = -30.0f;
    public float m_MaxPitch = 20.0f;
    [SerializeField] float m_Yaw;
    float m_MaxYaw = 360f;
    float m_MinYaw = 0f;

    [Header("Avoid Objects")]
    public LayerMask m_AvoidObjectsLayerMask;
    public float m_AvoidObjectsOffset = 0.1f;

    [Header("Debug")]
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public bool m_AngleLocked = false;
    bool m_MouseLocked = true;
    private bool m_UseYawInverted;
    private bool m_UsePitchInverted;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_MouseLocked = Cursor.lockState == CursorLockMode.Locked;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        resetCameraSave = m_TimetoResetCamera;

    }
#if UNITY_EDITOR
    void UpdateInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
        {
            m_AngleLocked = !m_AngleLocked;
        }
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_MouseLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif

    void LateUpdate()
    {
#if UNITY_EDITOR
        UpdateInputDebug();
#endif

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif
        transform.LookAt(m_LookAtTransform.position);
        float l_Distance = Vector3.Distance(transform.position, m_LookAtTransform.position);
        l_Distance = Mathf.Clamp(l_Distance, m_MinDistance, m_MaxDistance);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        float l_yaw = l_EulerAngles.y;

        m_Yaw += l_MouseX * m_YawRotationalSpeed * Time.deltaTime * (m_UseYawInverted ? -1.0f : 1.0f);
        m_Pitch += l_MouseY * m_PitchRotationalSpeed * Time.deltaTime * (m_UsePitchInverted ? -1.0f : 1.0f);
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);


        Vector3 l_ForwardCamera = new Vector3(Mathf.Sin(m_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad),
            Mathf.Sin(m_Pitch * Mathf.Deg2Rad), Mathf.Cos(m_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad));
        Vector3 l_DesiredPosition = m_LookAtTransform.position - l_ForwardCamera * l_Distance;

        Ray l_Ray = new Ray(m_LookAtTransform.position, -l_ForwardCamera);
        RaycastHit l_RayCastHit;
        if (Physics.Raycast(l_Ray, out l_RayCastHit, l_Distance, m_AvoidObjectsLayerMask.value))
            l_DesiredPosition = l_RayCastHit.point + l_ForwardCamera * m_AvoidObjectsOffset;

        transform.position = l_DesiredPosition;
        transform.LookAt(m_LookAtTransform.position);

        if (m_Yaw > 360f)
            m_Yaw = m_MinYaw;
        else if (m_Yaw < 0f)
            m_Yaw = m_MaxYaw;

        if (m_Player.m_PlayerMoving == false && (l_MouseX == 0 && l_MouseY == 0))
        {
            if (m_TimetoResetCamera > 0)
            {
                m_TimetoResetCamera -= Time.deltaTime;
            }

            else
            {
                m_Pitch = Mathf.Lerp(m_Pitch, -10f, 5f * Time.deltaTime);
                m_Yaw = Mathf.Lerp(m_Yaw, m_Player.transform.eulerAngles.y, 5f * Time.deltaTime);
                m_TimetoResetCamera = 0;
            }
        }

        else
        {
            RestartTimer();
        }

    }

    void RestartTimer()
    {
        m_TimetoResetCamera = resetCameraSave;
    }
}


