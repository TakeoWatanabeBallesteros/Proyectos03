using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSM_UIManager : MonoBehaviour
{
    private StateMachine uiManager_FSM;
    public Blackboard_UIManager blackboard_UIManager { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        blackboard_UIManager = GetComponent<Blackboard_UIManager>();
        uiManager_FSM = new StateMachine();
        AddStates();
        AddTransitions();
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
}
