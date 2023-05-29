using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_Playing : StateBase
{
    private Singleton singleton;

    public State_Playing() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.Playing);
        singleton.UIManager.blackboard_UIManager.InGameCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.InGameCanvas.SetActive(false);
        base.OnExit();
    }
}