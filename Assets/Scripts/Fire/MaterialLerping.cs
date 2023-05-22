using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerping : MonoBehaviour
{

    public bool canLerpMaterials;
    public Material materialObj;
    public Material materialRed;
    [SerializeField]Color initialColor;
    Material _objectMaterial;

    // Start is called before the first frame update
    void Start()
    {
        canLerpMaterials = false;
        _objectMaterial = GetComponent<MeshRenderer>().material;
        _objectMaterial.EnableKeyword("_EMISSION");
        initialColor = GetComponent<MeshRenderer>().material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {        
        if (canLerpMaterials)
        {
            _objectMaterial.Lerp(_objectMaterial, materialRed, Time.deltaTime/10f);
        }
    }
}
