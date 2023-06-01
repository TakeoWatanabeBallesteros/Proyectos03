using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer3D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public LayerMask mouseCollisionMask;
    private InputPlayerController playerInput;
    [SerializeField] Material objectMaterial;
    MeshRenderer meshRenderer;
    public Material weakWaterMaterial;
    public Material strongWaterMaterial;

    private void Start()
    {
        playerInput = Singleton.Instance.Player.GetComponent<InputPlayerController>();
        meshRenderer = GetComponent<MeshRenderer>();
        objectMaterial = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999, mouseCollisionMask))
        {
            transform.position = raycastHit.point;
        }
        else transform.position = Vector3.zero;

        if (playerInput.shoot)
        {
            objectMaterial.Lerp(objectMaterial, weakWaterMaterial, 4 * Time.deltaTime);
        }

        if (playerInput.secondaryShoot)
        {
            objectMaterial.Lerp(objectMaterial, strongWaterMaterial, 4 * Time.deltaTime);
        }
    }
    
}
