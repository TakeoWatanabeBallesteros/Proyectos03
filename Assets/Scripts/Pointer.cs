using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    Vector3 mouseScreenPos;
    Vector3 mosuseWorldPos;
    public LayerMask LM;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PositionMouse();
    }

    private void PositionMouse()
    {
        mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LM))
        {
            mosuseWorldPos = hit.point;
        }

        transform.position = mosuseWorldPos;
    }

}
