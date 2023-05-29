using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_LevelsMenu : StateBase
{
    private Singleton singleton;

    public State_LevelsMenu() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        singleton.GameManager.ChangeGameState(GameState.LvlsMenu);
        singleton.UIManager.blackboard_UIManager.LevelsMenuCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.LevelsMenuCanvas.SetActive(false);
        base.OnExit();
    }
}