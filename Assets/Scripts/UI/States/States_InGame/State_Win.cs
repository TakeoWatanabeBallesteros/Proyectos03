using FSM;
using UnityEngine;

public class State_Win : StateBase
{
    private Singleton singleton;

    public State_Win() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.Win);
        singleton.UIManager.blackboard_UIManager.WinCanvas.SetActive(true);
        singleton.GameManager.OnPause();
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.WinCanvas.SetActive(false);
        base.OnExit();
    }
}