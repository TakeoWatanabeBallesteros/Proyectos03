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
    }

    private void AddTransitions()
    {
        
    }
    #endregion
}