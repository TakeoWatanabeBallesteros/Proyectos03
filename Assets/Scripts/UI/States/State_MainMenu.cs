using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_MainMenu : StateBase
{
    private StateMachine uiManager_FSM;

    public State_MainMenu() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
