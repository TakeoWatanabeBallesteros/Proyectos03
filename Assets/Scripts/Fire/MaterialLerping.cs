using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerping : MonoBehaviour
{
    public bool canLerpMaterials;
    public Material materialRed;
    Material _objectMaterial;
    public float timeThatIsBurning;

    // Start is called before the first frame update
    void Start()
    {
        canLerpMaterials = false;
        _objectMaterial = GetComponentInChildren<MeshRenderer>().material;
        _objectMaterial.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {        
        if (canLerpMaterials)
        {
            _objectMaterial.Lerp(_objectMaterial, materialRed, Time.deltaTime/timeThatIsBurning);
        }
    }
}
