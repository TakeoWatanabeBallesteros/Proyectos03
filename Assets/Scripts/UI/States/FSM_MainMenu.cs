using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using FSM;

public class FSM_MainMenu : MonoBehaviour
{
    public StateMachine mainMenu_FSM { get; private set; }

    public Blackboard_UIManager blackboard_UIManager { get; private set; }

    private void Awake()
    {
        mainMenu_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        //mainMenu_FSM.SetStartState("MainMenu");
    }

    #region FSM Initialization
    private void AddStates()
    {
        
    }

    private void AddTransitions()
    {
        mainMenu_FSM.AddTriggerTransition("GoSettings", "MainMenu", "MenuSettings", forceInstantly: true);
    }
    #endregion

}
