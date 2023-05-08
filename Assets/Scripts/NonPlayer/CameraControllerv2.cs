using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControllerv2 : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    [SerializeField] private InputPlayerController controller;
    private Camera cam;

    public float maxCamSize;
    public float minCamSize;

    [SerializeField] float timer;
    float delay = 5f;

    private void Start()
    {
        cam = Camera.main;
        timer = delay;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.LookAt(target);
        }

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
        }
        
    }

    IEnumerator ResetCamera()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 13f, 2f * Time.deltaTime);        
        yield return new WaitForSeconds(2.5f);
        timer = delay;
    }
}
