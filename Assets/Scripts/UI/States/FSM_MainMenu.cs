using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using FSM;

public class FSM_MainMenu : MonoBehaviour
{
    public StateMachine mainMenu_FSM { get; private set; }

    private void Awake()
    {
        mainMenu_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        mainMenu_FSM.SetStartState("MainMenu");
    }

    #region FSM Initialization
    private void AddStates()
    {
        mainMenu_FSM.AddState("MainMenu", new State_MainMenu());
        mainMenu_FSM.AddState("SettingsMenu", new State_SettingsMenu());
        mainMenu_FSM.AddState("LevelsMenu", new State_LevelsMenu());
        mainMenu_FSM.AddState("Credits", new State_Credits());
    }

    private void AddTransitions()
    {
        mainMenu_FSM.AddTwoWayTriggerTransition("MainMenu-SettingsMenu","MainMenu", "SettingsMenu", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
        mainMenu_FSM.AddTwoWayTriggerTransition("MainMenu-LevelsMenu","MainMenu", "LevelsMenu", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
        mainMenu_FSM.AddTwoWayTriggerTransition("MainMenu-Credits","MainMenu", "Credits", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
    }
    #endregion

}
