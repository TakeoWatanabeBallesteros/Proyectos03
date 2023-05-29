using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_Credits : StateBase
{
    private Singleton singleton;

    public State_Credits() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        singleton.GameManager.ChangeGameState(GameState.Credits);
        singleton.UIManager.blackboard_UIManager.CreditsCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.CreditsCanvas.SetActive(false);
        base.OnExit();
    }
}