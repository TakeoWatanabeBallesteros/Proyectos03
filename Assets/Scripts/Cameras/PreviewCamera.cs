using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreviewCamera : MonoBehaviour
{
    private Camera _camera;
    private Animation _animation;
    public float animationLenght {get; private set;}

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _animation = GetComponent<Animation>();
        _camera.enabled = false;
        animationLenght = _animation.clip.length;
    }

    public void Play()
    {
        _camera.enabled = true;
        _animation.Play();
    }

    public void Stop()
    {
        _camera.enabled = false;
    }
}
