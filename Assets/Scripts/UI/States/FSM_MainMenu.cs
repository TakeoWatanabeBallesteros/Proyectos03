using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using FSM;

public class FSM_MainMenu : StateMachine
{

    public FSM_MainMenu(bool needsExitTime = true) : base(needsExitTime)
    {
        AddStates();
        AddTransitions();
        SetStartState("MainMenu");
    }
    
    #region FSM Initialization
    private void AddStates()
    {
        AddState("MainMenu", new State_MainMenu());
        AddState("SettingsMenu", new State_SettingsMenu());
        AddState("LevelsMenu", new State_LevelsMenu());
        AddState("Credits", new State_Credits());
    }

    private void AddTransitions()
    {
        this.AddTwoWayTriggerTransition("MainMenu-SettingsMenu","MainMenu", "SettingsMenu", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
        this.AddTwoWayTriggerTransition("MainMenu-LevelsMenu","MainMenu", "LevelsMenu", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
        this.AddTwoWayTriggerTransition("MainMenu-Credits","MainMenu", "Credits", t => Singleton.Instance.GameManager.gameState == GameState.MainMenu);
    }
    #endregion

}
