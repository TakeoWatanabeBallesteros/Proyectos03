using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraPreviewManager : MonoBehaviour
{
    private PreviewCamera [] cameraList;
    private Camera playerCamera;
    int currentCam = 0;

    //When the level begins all the cameras are disabled
    //The cameras get enabeled in order and they stay enabeled the number of seconds definied above, using a recursive corroutine
    //Finlaly when all the cameras have been shown the player camera is the only one enabeled

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraList = FindObjectsOfType<PreviewCamera>();
        Array.Reverse(cameraList);
        playerCamera.enabled = false;
        StartCoroutine(StartPreview());
    }

    IEnumerator StartPreview()
    {
        foreach (PreviewCamera camera in cameraList)
        {
            camera.Play();
            yield return new WaitForSeconds(camera.animationLenght);
            camera.Stop();
        }
        playerCamera.enabled = true;
    }
}
