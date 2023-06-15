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
        
    }
    
    // Update is called once per frame
    private void Update()
    {

    }
   

    public void Win()
    {
        Singleton.Instance.FinalScreenManager.CalculateStars();
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
    HowToPlay,
    ExitGame,
    LvlsMenu,
    LvlInfo,
    LevelPreview,
    Playing,
    Win,
    PauseMenu,
    SettingPause,
    RestartLvl
}

public enum LoadLevelType
{
    
}