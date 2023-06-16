using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterRecharge : MonoBehaviour
{
    public float waterPerSecond;
    
    private Blackboard_UIManager blackboardUIManager;
    private GoldWater waterPlayer;
    private PlayerInputController input;

    private delegate void RechargeDelegate();
    private RechargeDelegate recharge;

    private void Start() {
        blackboardUIManager = Singleton.Instance.UIManager.blackboard_UIManager;
    }
    
    private void Update() => recharge?.Invoke();
    
    private void GetWater(InputAction.CallbackContext context) {
        recharge = Recharge;
    }
    
    private void StopGetWater(InputAction.CallbackContext context) {
        recharge = null;
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
