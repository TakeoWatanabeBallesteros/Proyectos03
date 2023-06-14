using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float TimerEnSegundos;
    int minutes, seconds, cents;
    
    
    private Blackboard_UIManager blackboardUI;
    private bool Paused = true;
    
    
    private InputPlayerController inputPlayer;
    private MovementPlayerController playerMovement;
    private PlayerHealth playerHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;

        inputPlayer = Singleton.Instance.Player.GetComponent<InputPlayerController>();
        playerMovement = Singleton.Instance.Player.GetComponent<MovementPlayerController>();
        playerHealth = Singleton.Instance.Player.GetComponent<PlayerHealth>();
        
        PauseTimer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (Paused) return;
        TimerEnSegundos = Mathf.Clamp(TimerEnSegundos -= Time.deltaTime, 0, 999);
        if ( TimerEnSegundos == 0) {
            playerHealth.Dead = true;
            PauseTimer();
            StartCoroutine(blackboardUI.FadeIN());
            inputPlayer.enabled = false;
            playerMovement.Stop();
        }
        
        minutes = (int)(TimerEnSegundos / 60f);
        seconds = (int)(TimerEnSegundos - minutes * 60f);
        cents = (int)((TimerEnSegundos - (int)TimerEnSegundos) * 100f);

        blackboardUI.SetTimer($"{minutes:00}:{seconds:00}:{cents:00}"); 

    }
    public void PauseTimer()
    {
        Paused = true;
    }
    public void UnpauseTimer()
    {
        Paused = false;
    }
}
