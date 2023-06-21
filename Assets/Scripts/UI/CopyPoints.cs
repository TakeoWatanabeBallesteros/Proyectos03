using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopyPoints : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text MyTMP;
    public TMP_Text ParentTMP;
    void Awake()
    {
        MyTMP.fontSize = ParentTMP.fontSize;
    }

    // Update is called once per frame
    void Update()
    {

        MyTMP.text = ParentTMP.text;
        
    }
}
