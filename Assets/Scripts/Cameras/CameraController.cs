using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
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

    Vector3 originalPos;
       

    private void Start()
    {
        cam = Camera.main;
        timer = delay;
    }

    private void Update()
    {
        if (target != null)
        {            
            //transform.position = Vector3.Lerp(transform.position, target.position, 10f * Time.deltaTime);
            transform.position = target.position;
        }

        /*
        if (controller.zoom.y < 0)
        {
            timer = delay;
            cam.orthographicSize += 1f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, maxCamSize);
        }

        if (controller.zoom.y > 0)
        {
            timer = delay;
            cam.orthographicSize -= 1f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minCamSize, maxCamSize);
        }

        else if(controller.zoom.y == 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            StartCoroutine(ResetCamera());  
        }*/

    }

    IEnumerator ResetCamera()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 13f, 2f * Time.deltaTime);
        yield return new WaitForSeconds(2.5f);
        timer = delay;
    }
       

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = target.position + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }


}
