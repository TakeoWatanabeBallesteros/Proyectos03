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
    // Start is called before the first frame update
    void Start()
    {
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
    }

    // Update is called once per frame
    void Update()
    {
        TickTime();
    }
    private void TickTime()
    {
        if (Paused) return;
        if (TimerEnSegundos > 0) TimerEnSegundos -= Time.deltaTime;
        if ( TimerEnSegundos < 0)
        {
            TimerEnSegundos = 0;
            // Lose event GM
        }
        
        minutes = (int)(TimerEnSegundos / 60f);
        seconds = (int)(TimerEnSegundos - minutes * 60f);
        cents = (int)((TimerEnSegundos - (int)TimerEnSegundos) * 100f);

        blackboardUI.SetTimer($"{minutes:00}:{seconds:00}:{cents:00}"); 

    }
    public void StopTimer()
    {
        Paused = true;
    }
    public void ReanudateTimer()
    {
        Paused = false;
    }
}
