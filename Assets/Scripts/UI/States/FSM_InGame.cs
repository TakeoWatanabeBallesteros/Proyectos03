using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSM_InGame : MonoBehaviour
{
    public StateMachine inGame_FSM { get; private set; }

    private void Awake()
    {
        inGame_FSM = new StateMachine();
        AddStates();
        AddTransitions();
        //inGame_FSM.SetStartState("LevelPreview");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region FSM Initialization
    private void AddStates()
    {
        inGame_FSM.AddState("Playing", new State_Playing());
        inGame_FSM.AddState("LevelPreview", new State_LevelPreview());
    }

    private void AddTransitions()
    {
        
    }
    #endregion
}
