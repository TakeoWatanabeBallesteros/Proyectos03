using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraPreviewManager : MonoBehaviour
{
    private PreviewCamera [] cameraList; // All the Preview Cameras on the scene
    private Camera playerCamera; // Main Camera

    private void OnEnable()
    {
        if(Singleton.Instance == null) return;
        Singleton.Instance.GameManager.LevelPreviewStartEvent += LoadPreviewCameras;
    }

    void Start()
    {
        Singleton.Instance.GameManager.LevelPreviewStartEvent += LoadPreviewCameras;
        // playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // cameraList = FindObjectsOfType<PreviewCamera>();
        // Array.Reverse(cameraList);
        // playerCamera.enabled = false;
        // StartCoroutine(StartPreview());
    }

    /// <summary>
    /// Play all the Preview Cameras from the array, then enable the player camera.
    /// </summary>
    IEnumerator StartPreview()
    {
        foreach (PreviewCamera camera in cameraList)
        {
            camera.Play();
            yield return new WaitForSeconds(camera.animationLenght);
            camera.Stop();
        }
        playerCamera.enabled = true;

        Singleton.Instance.GameManager.LevelPreviewEnded();
    }
    
    /// <summary>
    /// Finds all the Preview Cameras from the scene and play them.
    /// </summary>
    public void LoadPreviewCameras()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraList = FindObjectsOfType<PreviewCamera>();
        Array.Reverse(cameraList);
        playerCamera.enabled = false;
        StartCoroutine(StartPreview());
    }
}
