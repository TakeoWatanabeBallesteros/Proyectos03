using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PickupKid : MonoBehaviour
{
    [SerializeField] private AnimatorController characterAnim;
    private GameObject pickupText;
    private bool CarringKid;
    [SerializeField] private bool CanPickup;
    private GameObject TargetKid;
    [SerializeField] private InputPlayerController playerInput;

    private MovementPlayerController movementPlayerController;
    public GameObject prefabPoseKid;

    private Blackboard_UIManager blackboardUI;
    
    // Start is called before the first frame update
    void Start()
    {
        CarringKid = false;
        CanPickup = false;

        playerInput = GetComponent<InputPlayerController>();
        movementPlayerController = GetComponent<MovementPlayerController>();
        prefabPoseKid.SetActive(false);

        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        pickupText = blackboardUI.PickUpText;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPickup && playerInput.interact)
        {
            if (CarringKid) return;
            blackboardUI.PickUpText.SetActive(false);
            Destroy(TargetKid);
            prefabPoseKid.SetActive(true);     
            CarringKid = true;
            CanPickup = false;
            movementPlayerController.speed *= 1.2f;
            blackboardUI.ChildFace();
            characterAnim.PickChild();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Kid") || CarringKid) return;
        blackboardUI.PickUpText.SetActive(false);
        CanPickup = true;
        pickupText.SetActive(true);
        TargetKid = other.transform.parent.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Kid")) return;
        blackboardUI.PickUpText.SetActive(false);
        CanPickup = false;
        pickupText.SetActive(false);
    }
    public bool HasKid()
    {
        return CarringKid;
    }
    public void KidYeet()
    {
        CarringKid = false;
        prefabPoseKid.SetActive(false);
    }
}
