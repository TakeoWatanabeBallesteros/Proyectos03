using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Unity.VisualScripting;

public class State_MainMenu : StateBase
{
    private Singleton singleton;
    
    public State_MainMenu() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.MainMenu);
        Debug.Log(singleton);
        singleton.UIManager.blackboard_UIManager.MainMenuCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.MainMenuCanvas.SetActive(false);
        base.OnExit();
    }
}