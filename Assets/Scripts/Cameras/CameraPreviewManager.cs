using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPreviewManager : MonoBehaviour
{
    private PreviewCamera [] cameraList; // All the Preview Cameras on the scene
    private Camera playerCamera; // Main Camera

    private void OnDisable()
    {
    }

    void Start()
    {
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
        EndPreview();
    }

    public void EndPreview()
    {
        StopAllCoroutines();
        playerCamera.enabled = true;
        foreach (PreviewCamera camera in cameraList)
        {
            camera.Stop();
        }
        Singleton.Instance.UIManager.uiManager_FSM.Trigger("LevelPreview-Playing");
    }
    
    /// <summary>
    /// Finds all the Preview Cameras from the scene and play them.
    /// </summary>
    public void LoadPreviewCameras()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraList = FindObjectsOfType<PreviewCamera>();
        if(!cameraList.Any()) 
        {
            Debug.Log("Yep");
            Singleton.Instance.UIManager.uiManager_FSM.Trigger("LevelPreview-Playing");
            return;
        }
        Array.Reverse(cameraList);
        playerCamera.enabled = false;
        StartCoroutine(StartPreview());
    }
}
