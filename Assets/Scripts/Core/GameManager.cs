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
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        /*
        Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        TotalCollectables = Collectables.Length;
 
        winScreen.SetActive(false);
        */
    }
    
    // Update is called once per frame
    private void Update()
    {
        /*
        NumberOfCollectables.text = ("Collectables: " + Collected + "/" + TotalCollectables);
    
        if(TimerEnSegundos>0 && SavedKids >= TotalKids)
        {
            Win();
        }
        */
        
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

    public void StartLevelPreview()
    {
        Singleton.Instance.GameManager.LevelPreviewStartEvent?.Invoke();
    }

    public void OnPause()
    {
        Time.timeScale = 0;
    }
    
    public void OnUnpause()
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
    SettingsMenu,
    Credits,
    ExitGame,
    LvlsMenu,
    LvlInfo,
    LevelPreview,
    Playing,
    PauseMenu,
    SettingPause,
    RestartLvl
}

public enum LoadLevelType
{
    
}