using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_PauseMenu : StateBase
{
private Singleton singleton;

public State_PauseMenu() : base(needsExitTime: false)
{
}
    
public override void OnEnter()
{
    singleton = Singleton.Instance;
    singleton.UIManager.blackboard_UIManager.PauseMenuCanvas.SetActive(true);
    singleton.GameManager.ChangeGameState(GameState.PauseMenu);
    singleton.GameManager.OnPause();
    base.OnEnter();
}
    
public override void OnLogic()
{
    base.OnLogic();
}

public override void OnExit()
{
    singleton.UIManager.blackboard_UIManager.PauseMenuCanvas.SetActive(false);
    base.OnExit();
}
}
