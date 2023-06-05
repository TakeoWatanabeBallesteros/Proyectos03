using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PickupKid : MonoBehaviour
{
    AnimatorController CharacterAnim;
    public GameManager GM;
    public Transform Shoulder;
    public GameObject DropText;
    private bool CarringKid;
    [SerializeField] private bool CanExtract;
    [SerializeField] private bool CanPickup;
    private GameObject TargetKid;
    [SerializeField] private InputPlayerController playerInput;

    private MovementPlayerController movementPlayerController;
    public GameObject prefabPoseKid;

    private Blackboard_UIManager blackboardUI;
    
    // Start is called before the first frame update
    void Start()
    {
        CharacterAnim = GetComponent<AnimatorController>();
        CarringKid = false;
        CanPickup = false;

        playerInput = GetComponent<InputPlayerController>();
        movementPlayerController = GetComponent<MovementPlayerController>();
        prefabPoseKid.SetActive(false);

        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPickup && playerInput.interact)
        {
            blackboardUI.PickUpText.SetActive(false);
            Destroy(TargetKid);
            prefabPoseKid.SetActive(true);     
            CarringKid = true;
            CanPickup = false;
            movementPlayerController.speed *= 1.2f;
            CharacterAnim.PickChild();
            blackboardUI.ChildFace();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kid") && !CarringKid)
        {
            blackboardUI.PickUpText.SetActive(false);
            CanPickup = true;
            TargetKid = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Kid")) return;
        blackboardUI.PickUpText.SetActive(false);
        CanPickup = false;
        CanExtract = false;
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
