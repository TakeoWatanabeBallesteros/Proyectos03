using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameState gameState { get; private set; }
    
    [SerializeField] private int SavedKids = 0;
    [SerializeField] private GameObject[] Kids;
    [SerializeField] private int TotalKids = 0;

    [SerializeField] private int Collected = 0;
    [SerializeField] private GameObject[] Collectables;
    [SerializeField] private int TotalCollectables = 0;

    PlayerHealth PH;
    public TMP_Text NumberOfkids;
    public TMP_Text NumberOfCollectables;
    public TMP_Text TimeLeftText;
    [SerializeField] private float TimerEnSegundos;
    int minutes, seconds, cents;
    public GameObject winScreen;
    
    // That maybe should not be here
    private PlayerControls controls = null;
    public event Action PauseEvent;
    public event Action UnpauseEvent;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Player.Pause.performed += ctx =>
        {
            switch (Singleton.Instance.GameManager.gameState)
            {
                case GameState.PauseMenu:
                    Singleton.Instance.GameManager.gameState = GameState.InGame;
                    UnpauseEvent?.Invoke();
                    break;
                case GameState.InGame:
                    Singleton.Instance.GameManager.gameState = GameState.PauseMenu;
                    PauseEvent?.Invoke();
                    break;
            }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.GameManager.gameState = GameState.InGame;
        Singleton.Instance.GameManager.PauseEvent += OnPause;
        Singleton.Instance.GameManager.UnpauseEvent += OnUnpause;
        
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        DontDestroyOnLoad(this);
 
        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;

        Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        TotalCollectables = Collectables.Length;
 
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
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
        NumberOfCollectables.text = ("Collectables: " + Collected + "/" + TotalCollectables);
    
        if(TimerEnSegundos>0 && SavedKids >= TotalKids)
        {
            Win();
        }
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
    public void AddCollectable()
    {
        Collected++;
    }
    public void AddTime(float Time)
    {
        TimerEnSegundos += Time;
    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    private void OnPause()
    {
        Time.timeScale = 0;
    }
    
    private void OnUnpause()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

public enum GameState{
    MainMenu,
    PauseMenu,
    InGame
}