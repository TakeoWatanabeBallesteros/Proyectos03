using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class State_WinningStars : StateBase
{
    private Singleton singleton;
    public State_WinningStars() : base(needsExitTime : false)
    {
    }
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.StarsMenu);
        singleton.UIManager.blackboard_UIManager.StarsGameCanvas.SetActive(true);
        singleton.GameManager.OnPause();
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.StarsGameCanvas.SetActive(false);
        base.OnExit();
    }

    
}
