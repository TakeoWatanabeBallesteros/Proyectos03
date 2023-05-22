using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FSM;

public class FSM_UIManager : MonoBehaviour
{
    public StateMachine uiManager_FSM { get; private set; }
    public Blackboard_UIManager blackboard_UIManager { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        blackboard_UIManager = GetComponent<Blackboard_UIManager>();
        uiManager_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        uiManager_FSM.SetStartState("MainMenu_FSM");
        uiManager_FSM.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region FSM Initialization

    private void AddStates()
    {
        uiManager_FSM.AddState("MainMenu_FSM", GetComponent<FSM_MainMenu>().mainMenu_FSM);
        uiManager_FSM.AddState("InGame_FSM", GetComponent<FSM_InGame>().inGame_FSM);
    }

    private void AddTransitions()
    {

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
