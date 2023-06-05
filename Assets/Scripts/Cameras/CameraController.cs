using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    public float distance = 10f;
    [SerializeField] private InputPlayerController controller;
    private Camera cam;

    public float maxCamSize;
    public float minCamSize;

    [SerializeField] float timer;
    float delay = 5f;

    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public Vector3 addPosition;

    private Vector3 startPos;

    private void Start()
    {
        cam = Camera.main;
        timer = delay;
        // target = Singleton.Instance.Player.transform;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + addPosition;
        }            

    }

    private IEnumerator ResetCamera()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 13f, 2f * Time.deltaTime);
        yield return new WaitForSeconds(2.5f);
        timer = delay;
    }
       

    private void LateUpdate()
    {
        if (!(shakeDuration > 0)) return;
        // camTransform.localPosition = target.position + addPosition + Random.insideUnitSphere * shakeAmount;

        shakeDuration -= Time.deltaTime * decreaseFactor;
    }


}
