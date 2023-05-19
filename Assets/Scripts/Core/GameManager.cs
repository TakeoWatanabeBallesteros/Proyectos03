using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    
    //TODO: The timer should be in another component.
    //TODO: Kids counter should be in another component.
    //TODO: Collectables should be in another component.

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

    #region Events
    //Event for debug the game state, remove on Gold.
    public event Action GameStateChangedEvent;
    public event Action PauseEvent;
    public event Action UnpauseEvent;
    public event Action LevelPreviewStartEvent;
    public event Action LevelPreviewEndEvent;
    public event Action RestartEvent;
    #endregion

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
        Singleton.Instance.GameManager.PauseEvent += OnPause;
        Singleton.Instance.GameManager.UnpauseEvent += OnUnpause;
        
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
 
        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;

        Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        TotalCollectables = Collectables.Length;
 
        winScreen.SetActive(false);
        
        Singleton.Instance.GameManager.gameState = GameState.LevelPreview;
        Singleton.Instance.GameManager.LevelPreviewStartEvent?.Invoke();
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

    public void LevelPreviewEnded(){
        Singleton.Instance.GameManager.LevelPreviewEndEvent?.Invoke();
        Singleton.Instance.GameManager.gameState = GameState.InGame;
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
        RestartEvent?.Invoke();
        SceneManager.LoadScene(0);
    }

    public void ChangeGameState(GameState state)
    {
        Singleton.Instance.GameManager.gameState = state;
        Singleton.Instance.GameManager.GameStateChangedEvent?.Invoke();
    }
}

public enum GameState{
    MainMenu,
    SettingMenu,
    Credits,
    ExitGame,
    LvlsMenu,
    LvlInfo,
    LevelPreview,
    InGame,
    PauseMenu,
    SettingPause,
    RestartLvl
}