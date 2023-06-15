using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSM_InGame : StateMachine
{

    public FSM_InGame(bool needsExitTime = true) : base(needsExitTime)
    {
        AddStates();
        AddTransitions();
        SetStartState("LevelPreview");
    }

    #region FSM Initialization
    private void AddStates()
    {
        AddState("LevelPreview", new State_LevelPreview());
        AddState("Playing", new State_Playing());
        AddState("Win", new State_Win());
        AddState("PauseMenu", new State_PauseMenu());
        AddState("PauseMenuSettings", new State_PauseMenuSettings());
    }

    private void AddTransitions()
    {
        this.AddTriggerTransition("LevelPreview-Playing","LevelPreview", "Playing", t => true);
        this.AddTriggerTransition("Playing-Win","Playing", "Win", t => true);
        this.AddTwoWayTriggerTransition("Playing-PauseMenu","Playing", "PauseMenu", t => Singleton.Instance.GameManager.gameState == GameState.Playing);
        this.AddTwoWayTriggerTransition("PauseMenu-PauseMenuSettings","PauseMenu", "PauseMenuSettings", t => Singleton.Instance.GameManager.gameState == GameState.PauseMenu);
    }
    #endregion
}
