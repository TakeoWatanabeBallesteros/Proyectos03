using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TMP_Text TimeLeftText;
    [SerializeField] private float TimerEnSegundos;
    int minutes, seconds, cents;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerEnSegundos > 0) TimerEnSegundos -= Time.deltaTime;
        if ( TimerEnSegundos < 0)
        {
            TimerEnSegundos = 0;
            // Lose event GM
        }
        
        minutes = (int)(TimerEnSegundos / 60f);
        seconds = (int)(TimerEnSegundos - minutes * 60f);
        cents = (int)((TimerEnSegundos - (int)TimerEnSegundos) * 100f);

        TimeLeftText.text = $"{minutes:00}:{seconds:00}:{cents:00}";
    }
}
