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

    #region Events
    //Event for debug the game state, remove on Gold.
    public event Action GameStateChangedEvent;
    #endregion

    public void Win() {
        Singleton.Instance.FinalScreenManager.CalculateStars();
    }

    public static void OnPause() => Time.timeScale = 0;
    
    public static void OnUnpause() => Time.timeScale = 1;

    public static void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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