using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_SettingsMenu : StateBase
{
    private Singleton singleton;
    
    public State_SettingsMenu() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.SettingsMenu);
        singleton.UIManager.blackboard_UIManager.SettingsMenuCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.SettingsMenuCanvas.SetActive(false);
        base.OnExit();
    }
}