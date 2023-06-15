using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;
using FSM;
using FMODUnity;

[RequireComponent(typeof(Blackboard_UIManager))]
public class FSM_UIManager : MonoBehaviour
{
    public StateMachine uiManager_FSM { get; private set; }
    public Blackboard_UIManager blackboard_UIManager;

    [SerializeField] private EventReference inGameMusic;
    public EventInstance _inGameMusic { get; private set; }

    public bool testingLevel = false;
    
    private void Awake()
    {
        blackboard_UIManager = GetComponent<Blackboard_UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _inGameMusic = RuntimeManager.CreateInstance(inGameMusic);
        uiManager_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        uiManager_FSM.SetStartState(testingLevel ? "InGame_FSM" : "MainMenu_FSM");
        uiManager_FSM.Init();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name[0] == 'L')
        {
            uiManager_FSM.Trigger("MainMenu_FSM-InGame_FSM");
        }
    }

    #region FSM Initialization

    private void AddStates()
    {
        uiManager_FSM.AddState("MainMenu_FSM", new FSM_MainMenu());
        uiManager_FSM.AddState("InGame_FSM", new FSM_InGame());
    }

    private void AddTransitions()
    {
        uiManager_FSM.AddTwoWayTriggerTransition("MainMenu_FSM-InGame_FSM","MainMenu_FSM", "InGame_FSM", t => uiManager_FSM.ActiveState.name == "MainMenu_FSM");
    }

    #endregion

    #region Buttons Methods

    public void MainMenuSettings()
    {
        uiManager_FSM.Trigger("MainMenu-SettingsMenu");
    }
    
    public void MainMenuLevelsMenu()
    {
        uiManager_FSM.Trigger("MainMenu-LevelsMenu");
    }

    public void MainMenuCredits()
    {
        uiManager_FSM.Trigger("MainMenu-Credits");
    }

    // The LoadSceneAsync method maybe will be in the GameManager.
    public void GoLevel(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoMainMenu()
    {
        uiManager_FSM.Trigger("MainMenu_FSM-InGame_FSM");
        SceneManager.LoadSceneAsync(0);
    }
    public void GoInGame()
    {
        uiManager_FSM.Trigger("Playing-PauseMenu");
    }

    public void WantToExit()
    {
        Singleton.Instance.UIManager.blackboard_UIManager.ExitGameCanvas.SetActive(true);
        Singleton.Instance.GameManager.ChangeGameState(GameState.ExitGame);
    }

    public void SureToExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void NotSureToExit()
    {
        Singleton.Instance.UIManager.blackboard_UIManager.ExitGameCanvas.SetActive(false);
        Singleton.Instance.GameManager.ChangeGameState(GameState.MainMenu);
    }

    #endregion
}
