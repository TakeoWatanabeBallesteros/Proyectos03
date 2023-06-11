using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WinningImage : MonoBehaviour
{
    public Image winImage;
    
    private void Awake()
    {
        winImage = GetComponent<Image>();
        winImage.transform.localScale = Vector3.zero;
    }
}
