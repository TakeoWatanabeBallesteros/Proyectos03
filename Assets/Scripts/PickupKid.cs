using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PickupKid : MonoBehaviour
{
    public GameController GM;
    public Transform Shoulder;
    public Transform DropPoint;
    public TMP_Text PickupText;
    private bool CarringKid;
    [SerializeField] private bool CanExtract;
    [SerializeField]private bool CanPickup;
    private GameObject TargetKid;
    [SerializeField] private PlayerInput playerInput;

    private MovementPlayerController _movementPlayerController;

    // Start is called before the first frame update
    void Start()
    {
        CarringKid = false;
        CanExtract = false;
        CanPickup = false;

        _movementPlayerController = GetComponent<MovementPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void OnInteract(InputValue valor)
    {
        if (CanPickup)
        {
            PickupText.enabled = false;
            TargetKid.transform.SetParent(gameObject.transform);
            TargetKid.transform.position = Shoulder.position;
            TargetKid.transform.rotation = Quaternion.Euler(-90, 0, 0);
            TargetKid.GetComponent<BoxCollider>().enabled = false;
            CarringKid = true;
            CanPickup = false;
            _movementPlayerController.speed /= 2;
        }
        if (CanExtract)
        {
            PickupText.enabled = false;
            TargetKid.transform.SetParent(null);
            TargetKid.transform.position = DropPoint.position;
            TargetKid.transform.rotation = Quaternion.Euler(-90, 90, 0);
            CarringKid = false;
            GM.AddChild();
            CanExtract = false;
            _movementPlayerController.speed *= 2;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag ==  "Kid" && !CarringKid)
        {
            PickupText.text = "Press Space to pickup Kid";
            PickupText.enabled = true;
            CanPickup = true;
            TargetKid = other.gameObject;
        }
        if (other.tag == "Extraction" && CarringKid)
        {
            PickupText.text = "Press Space to Drop Kid";
            PickupText.enabled = true;
            CanExtract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kid" || other.tag == "Extraction")
        {
            PickupText.enabled = false;
            CanPickup = false;
            CanExtract = false;
        }

    }
    public bool HasKid()
    {
        return CarringKid;
    }
}
