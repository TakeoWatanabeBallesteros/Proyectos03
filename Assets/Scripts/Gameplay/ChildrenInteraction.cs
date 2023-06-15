using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenInteraction : MonoBehaviour
{
    [SerializeField] private AnimatorController character_Animator;
    PlayerControls controls;
    
    [Header("Pick Up")]
    private GameObject TargetKid;
    private GameObject pickup_Text;
    private bool carring_Kid;
    
    void Start() {
        controls = controls ?? new PlayerControls();
        controls.Enable();
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Kid") && !carring_Kid && TargetKid == null) {
            TargetKid = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        
    }
}
