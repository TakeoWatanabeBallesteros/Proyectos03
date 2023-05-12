using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPreviewManager : MonoBehaviour
{
    public CameraPreview [] cameraList;
    public GameObject playerCam;
    int currentCam = 0;

    //When the level begins all the cameras are disabled
    //The cameras get enabeled in order and they stay enabeled the number of seconds definied above, using a recursive corroutine
    //Finlaly when all the cameras have been shown the player camera is the only one enabeled
   
    void Start()
    {
        playerCam.SetActive(false);
        for (int i = 0; i < cameraList.Length; i++)
        {
            cameraList[i].cam.SetActive(false);
        }
        StartCoroutine(Cameras());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    IEnumerator Cameras()
    {
        cameraList[currentCam].cam.enabled = true;
        yield return new WaitForSeconds(cameraList[currentCam].timeOfView);
        cameraList[currentCam].cam.enabled = false;
        currentCam++;
        if (currentCam < cameraList.Length)
        {
            StartCoroutine(Cameras());
        }
        else
        {
            playerCam.enabled = true;
        }
    }
    */

    IEnumerator Cameras()
    {
        foreach (CameraPreview cam in cameraList)
        {
            cam.cam.SetActive(true);
            yield return new WaitForSeconds(cam.timeOfView);
            cam.cam.SetActive(false);
        }
        playerCam.SetActive(true);
    }
}


[System.Serializable]
public class CameraPreview{
    public GameObject cam;
    public float timeOfView;
}
