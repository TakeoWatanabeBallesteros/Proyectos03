using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KidPickAndThrow : MonoBehaviour
{
    [SerializeField] private AnimatorController character_Animator;
    [SerializeField] private PlayerMovementController player_Movement;
    [SerializeField] private PlayerInputController input;

    [Header("Pick Up")] 
    [SerializeField] private GameObject kid_Picked_Prefab; // Should be a list
    private GameObject TargetKid;
    private GameObject pickup_Text;
    private bool carring_Kid;

    [Space(5)] 
    [Header("Throw Kid")] 
    [SerializeField] private GameObject kid_Ragdoll_Prefab;
    [SerializeField] private Transform shoulder;
    [SerializeField] private float max_Power;
    [SerializeField] private float power_Per_Second;
    private Slider power_Slider;
    private float power;

    private KidType kid_Type;
    
    private Blackboard_UIManager blackboard_UI;
    
    private delegate void GetPowerDelegate();
    private GetPowerDelegate get_Power;
    
    private void Start()
    {
        blackboard_UI = Singleton.Instance.UIManager.blackboard_UIManager;
        pickup_Text = blackboard_UI.PickUpText;
        power_Slider = blackboard_UI.powerBar;
    }

    private void Update() => get_Power?.Invoke();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Kid") || carring_Kid || TargetKid != null) return;
        TargetKid = other.transform.parent.gameObject;
        kid_Type = other.GetComponent<KidHealthBehavior>().kidType;
        input.AddInteractFunction(PickupKid);
        pickup_Text.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Kid")) return;
        TargetKid = null;
        input.RemoveInteractFunction(PickupKid);
        pickup_Text.SetActive(false);
    }

    private void PickupKid(InputAction.CallbackContext ctx) {
        pickup_Text.SetActive(false);
        Destroy(TargetKid);
        TargetKid = null;
        kid_Picked_Prefab.SetActive(true); //TODO: Use a function to chose between gender
        if (kid_Type.gender == KidGender.Male) {
            Debug.Log(kid_Type.diff == KidDiff.Diff1 ? "Chose male 1" : "Chose male 2");
        }
        else {
            Debug.Log(kid_Type.diff == KidDiff.Diff1 ? "Chose female 1" : "Chose female 2");
        }
        carring_Kid = true;
        player_Movement.speed = player_Movement.carryingSpeed;
        blackboard_UI.ChildFace();
        character_Animator.PickChild();
        input.AddThrowFunction(SetGetPower);
        input.AddThrowCanceledFunction(Throw);
    }

    private void SetGetPower(InputAction.CallbackContext ctx) {
        if(input.shoot) return;
        get_Power = GetPower;
        character_Animator.PrepareChild(true);
    }

    private void GetPower() {
        power_Slider.gameObject.SetActive(true);
        power = Mathf.Clamp(power += power_Per_Second * Time.deltaTime, 0, max_Power);
        power_Slider.value = power;
    }

    private void Throw(InputAction.CallbackContext ctx) {
        if(input.shoot) return;
        get_Power = null;
        character_Animator.PrepareChild(false);
        var kid = Instantiate(kid_Ragdoll_Prefab);
        kid.transform.position = shoulder.position;
        kid.transform.Find("Root").GetComponent<Rigidbody>().AddForce(transform.forward.normalized * power, ForceMode.Impulse);
        carring_Kid = false;
        kid_Picked_Prefab.SetActive(false);
        player_Movement.speed = player_Movement.Maxspeed;
        power = 0;
        power_Slider.gameObject.SetActive(false);
        power_Slider.value = power;
        input.RemoveThrowFunction(SetGetPower);
        input.RemoveThrowCanceledFunction(Throw);
    }

    public void ForgetKid() {
        TargetKid = null;
        input.RemoveInteractFunction(PickupKid);
        pickup_Text.SetActive(false);
    }
}
