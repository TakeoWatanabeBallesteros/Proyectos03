using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterRecharge : MonoBehaviour
{
    private Blackboard_UIManager blackboardUIManager;
    public float waterPerSecond;
    [SerializeField] PlayerControls Controls;
    private GoldWater waterPlayer;
    private bool onRecharge;

   
    private void Start()
    {
        Controls = Controls ?? new PlayerControls();
        blackboardUIManager = Singleton.Instance.UIManager.blackboard_UIManager;
        Controls.Enable();
        onRecharge = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blackboardUIManager.ReloadText.SetActive(true);
            Controls.Player.Recharge.started += Luky;
            Controls.Player.Recharge.canceled += Ramon; 
            waterPlayer = waterPlayer ?? other.GetComponentInChildren<GoldWater>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blackboardUIManager.ReloadText.SetActive(false);
            Controls.Player.Recharge.started -= Luky;
            Controls.Player.Recharge.canceled += Ramon;
            StopAllCoroutines();
        }
    }

    private void Luky(InputAction.CallbackContext context)
    {
        StartCoroutine(GetWater(context)); 
    }

    private void Ramon(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
    }

    IEnumerator GetWater(InputAction.CallbackContext context)
    {
        while (waterPlayer.GetCurrentWater() < 100 && !context.canceled)
        {
            waterPlayer.Recharge(waterPerSecond * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
