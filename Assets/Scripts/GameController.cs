using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private int SavedKids = 0;
    [SerializeField] private GameObject[] Kids;
    [SerializeField] private int TotalKids = 0;
    PlayerHealth PH;
    public TMP_Text NumberOfkids;
    public TMP_Text TimeLeftText;
    [SerializeField] private float TimerEnSegundos;
    int minutes, seconds, cents;

    // Start is called before the first frame update
    void Start()
    {
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        DontDestroyOnLoad(this);
        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerEnSegundos > 0)
            TimerEnSegundos -= Time.deltaTime;
        if ( TimerEnSegundos < 0)
            TimerEnSegundos = 0;
        if (TimerEnSegundos == 0)
            PH.IntantDeath();
        minutes = (int)(TimerEnSegundos / 60f);
        seconds = (int)(TimerEnSegundos - minutes * 60f);
        cents = (int)((TimerEnSegundos - (int)TimerEnSegundos) * 100f);

        TimeLeftText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
        NumberOfkids.text = ("Kids saved: " + SavedKids + "/" + TotalKids);
    }
    public int GetTotalKids()
    {
        return TotalKids;
    }
    public int GetSavedKids()
    {
        return TotalKids;
    }
    public void AddChild()
    {
        SavedKids++;
    }
    public void AddTime(float Time)
    {
        TimerEnSegundos += Time;
    }
}
