using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreviewCamera : MonoBehaviour
{
    private Camera camera;
    private Animation animation;
    public float animationLenght {get; private set;}

    private void Awake()
    {
        camera = GetComponent<Camera>();
        animation = GetComponent<Animation>();
        camera.enabled = false;
        animationLenght = animation.clip.length;
    }

    public void Play()
    {
        camera.enabled = true;
        animation.Play();
    }

    public void Stop()
    {
        camera.enabled = false;
    }
}
