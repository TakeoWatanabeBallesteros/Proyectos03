using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PickupKid : MonoBehaviour
{
    public GameManager GM;
    public Transform Shoulder;
    public Transform DropPoint;
    public GameObject PickupText;
    public GameObject DropText;
    private bool CarringKid;
    [SerializeField] private bool CanExtract;
    [SerializeField] private bool CanPickup;
    private GameObject TargetKid;
    [SerializeField] private InputPlayerController playerInput;

    private MovementPlayerController _movementPlayerController;
    public GameObject prefabPoseKid;

    // Start is called before the first frame update
    void Start()
    {
        CarringKid = false;
        CanExtract = false;
        CanPickup = false;

        playerInput = GetComponent<InputPlayerController>();
        _movementPlayerController = GetComponent<MovementPlayerController>();
        prefabPoseKid.SetActive(false);

        GM = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.interact)
        {
            if (CanPickup)
            {
                PickupText.SetActive(false);
                TargetKid.SetActive(false);
                prefabPoseKid.SetActive(true);                
                CarringKid = true;
                CanPickup = false;
                _movementPlayerController.speed *= 1.2f;
            }
            if (CanExtract)
            {
                DropText.SetActive(false);
                prefabPoseKid.SetActive(false);
                TargetKid.SetActive(true);
                TargetKid.tag = "KidExtracted";
                TargetKid.transform.position = DropPoint.position;
                //TargetKid.transform.rotation = DropPoint.rotation;
                CarringKid = false;
                GM.AddChild();
                CanExtract = false;
                _movementPlayerController.speed /= 1.2f;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kid" && !CarringKid)
        {
            PickupText.SetActive(true);
            CanPickup = true;
            TargetKid = other.gameObject;
        }
        if (other.tag == "Extraction" && CarringKid)
        {
            DropText.SetActive(true);
            CanExtract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Kid" && other.tag != "Extraction") return;
        PickupText.SetActive(false);
        PickupText.SetActive(false);
        CanPickup = false;
        CanExtract = false;
    }
    public bool HasKid()
    {
        return CarringKid;
    }
}
