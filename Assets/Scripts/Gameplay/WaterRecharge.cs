using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterRecharge : MonoBehaviour
{
    public float waterPerSecond;
    
    private Blackboard_UIManager blackboardUIManager;
    private GoldWater waterPlayer;
    private PlayerInputController input;

    FMOD.Studio.EventInstance waterRecharge;

    private Animator Anim;
    private delegate void RechargeDelegate();
    private RechargeDelegate recharge;

    private void Start() {
        Anim = GetComponentInChildren<Animator>();
        blackboardUIManager = Singleton.Instance.UIManager.blackboard_UIManager;
        waterRecharge = FMODUnity.RuntimeManager.CreateInstance("event:/Water/Water refilling at Recharging Stations");
    }
    
    private void Update() => recharge?.Invoke();
    
    private void GetWater(InputAction.CallbackContext context) {
        recharge = Recharge;
        waterRecharge.start();
        Anim.SetBool("Recharge", true);
    }
    
    private void StopGetWater(InputAction.CallbackContext context) {
        recharge = null;
        waterRecharge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Anim.SetBool("Recharge", false);
    }
    
    private void Recharge() {
        waterPlayer.Recharge(waterPerSecond * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        blackboardUIManager.ReloadText.SetActive(true);
        waterPlayer = waterPlayer ?? other.GetComponentInChildren<GoldWater>();
        input = input ?? other.GetComponent<PlayerInputController>();
        input.AddRechargeFunction(GetWater);
        input.AddRechargeCanceledFunction(StopGetWater);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        blackboardUIManager.ReloadText.SetActive(false);
        input.RemoveRechargeFunction(GetWater);
        input.RemoveRechargeCanceledFunction(StopGetWater);
        recharge = null;
    }
}
