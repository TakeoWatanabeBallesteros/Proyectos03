using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Serialization;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float remainingTime;
    private int minutes, seconds, cents;
    
    private bool paused;
    
    private Blackboard_UIManager blackboardUI;
    private PlayerInputController input;
    private PlayerMovementController playerMovement;
    private PlayerHealth playerHealth;
    
    void Start() {
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        input = Singleton.Instance.Player.GetComponent<PlayerInputController>();
        playerMovement = Singleton.Instance.Player.GetComponent<PlayerMovementController>();
        playerHealth = Singleton.Instance.Player.GetComponent<PlayerHealth>();
        
        PauseTimer();
    }

    void Update() => UpdateTimer();
    
    private void UpdateTimer() {
        if (paused) return;
        
        remainingTime = Mathf.Clamp(remainingTime -= Time.deltaTime, 0, 999);
        if ( remainingTime == 0) { //TODO: Move to GameManager
            playerHealth.Dead = true;
            PauseTimer();
            StartCoroutine(blackboardUI.FadeIN(blackboardUI.TimesUpImage));
            playerMovement.Stop();
            input.AddSpaceFunction(GoMainMenu);
        }
        
        minutes = (int)(remainingTime / 60f);
        seconds = (int)(remainingTime - minutes * 60f);
        cents = (int)((remainingTime - (int)remainingTime) * 100f);

        blackboardUI.SetTimer($"{minutes:00}:{seconds:00}:{cents:00}");
    }

    public void PauseTimer() => paused = true;

    public void UnpauseTimer() => paused = false;

    private void GoMainMenu(InputAction.CallbackContext ctx) {
        Singleton.Instance.UIManager.blackboard_UIManager.TimesUpImage.color = new Color(1f, 1f, 1f, 0);
        Singleton.Instance.UIManager.GoMainMenu();
        input.RemoveSpaceFunction(GoMainMenu);
    }
    
}
