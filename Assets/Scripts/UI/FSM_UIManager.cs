using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;
using FSM;
using FMODUnity;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Blackboard_UIManager))]
public class FSM_UIManager : MonoBehaviour
{
    public StateMachine uiManager_FSM { get; private set; }
    public Blackboard_UIManager blackboard_UIManager;
    public GameManager gameManager;

    [SerializeField] private EventReference inGameMusic;
    public EventInstance _inGameMusic { get; private set; }

    public bool testingLevel;
    
    private void Awake() {
        blackboard_UIManager = GetComponent<Blackboard_UIManager>();
    }

    // Start is called before the first frame update
    void Start() {
        _inGameMusic = RuntimeManager.CreateInstance(inGameMusic);
        uiManager_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        uiManager_FSM.SetStartState(testingLevel ? "InGame_FSM" : "MainMenu_FSM");
        uiManager_FSM.Init();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name[0] == 'L') {
            uiManager_FSM.Trigger("MainMenu_FSM-InGame_FSM");
        }
        else {
            uiManager_FSM.Trigger("MainMenu_FSM-InGame_FSM");
        }
    }

    #region FSM Initialization

    private void AddStates() {
        uiManager_FSM.AddState("MainMenu_FSM", new FSM_MainMenu());
        uiManager_FSM.AddState("InGame_FSM", new FSM_InGame());
    }

    private void AddTransitions() => uiManager_FSM.AddTwoWayTriggerTransition("MainMenu_FSM-InGame_FSM",
        "MainMenu_FSM", "InGame_FSM", t => uiManager_FSM.ActiveState.name == "MainMenu_FSM");

    #endregion

    #region Buttons Methods
    public void MainMenuSettings() => uiManager_FSM.Trigger("MainMenu-SettingsMenu");
    
    public void MainMenuLevelsMenu() => uiManager_FSM.Trigger("MainMenu-LevelsMenu");

    public void MainMenuCredits() => uiManager_FSM.Trigger("MainMenu-Credits");
    
    public void MainMenuHowToPlay() => uiManager_FSM.Trigger("MainMenu-HowToPlay");
    
    public void GoLevel(int index) => SceneManager.LoadSceneAsync(index);
    
    public void NextLevel() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

    public void RestartLevel() => GameManager.Restart();

    public void GoMainMenu() => SceneManager.LoadSceneAsync(0);
    
    public void GoInGame() => uiManager_FSM.Trigger("Playing-PauseMenu");
    
    public void GoPauseSettings() => uiManager_FSM.Trigger("PauseMenu-PauseMenuSettings");

    public void WantToExit() {
        Singleton.Instance.UIManager.blackboard_UIManager.ExitGameCanvas.SetActive(true);
        Singleton.Instance.GameManager.ChangeGameState(GameState.ExitGame);
    }

    public void SureToExit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void NotSureToExit() {
        Singleton.Instance.UIManager.blackboard_UIManager.ExitGameCanvas.SetActive(false);
        Singleton.Instance.GameManager.ChangeGameState(GameState.MainMenu);
    }

    public void OnEscPressed(InputAction.CallbackContext ctx) {
        switch (gameManager.gameState) {
            case GameState.PauseMenu:
                uiManager_FSM.Trigger("Playing-PauseMenu");
                break;
            case GameState.Playing: // You trigger between Game & Pause
                uiManager_FSM.Trigger("Playing-PauseMenu");
                break;
            case GameState.MainMenu:
                WantToExit();
                break;
            case GameState.SettingsMenu:
                uiManager_FSM.Trigger("MainMenu-SettingsMenu");
                break;
            case GameState.Credits:
                uiManager_FSM.Trigger("MainMenu-Credits");
                break;
            case GameState.HowToPlay:
                uiManager_FSM.Trigger("MainMenu-HowToPlay");
                break;
            case GameState.ExitGame:
                NotSureToExit();
                break;
            case GameState.LvlsMenu:
                uiManager_FSM.Trigger("MainMenu-LevelsMenu");
                break;
            case GameState.LvlInfo:
                blackboard_UIManager.LevelInfoCanvas.SetActive(false);
                break;
            case GameState.LevelPreview:
                Singleton.Instance.CameraPreviewManager.EndPreview();
                break;
            case GameState.SettingPause:
                uiManager_FSM.Trigger("PauseMenu-PauseMenuSettings");
                break;
            case GameState.RestartLvl:
                break;
            case GameState.Win:
                break;
        }
    }
    #endregion
}
