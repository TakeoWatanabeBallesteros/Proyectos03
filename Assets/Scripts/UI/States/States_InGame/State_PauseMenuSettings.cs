using FSM;

public class State_PauseMenuSettings : StateBase
{
    private Singleton singleton;

    public State_PauseMenuSettings() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        singleton.UIManager.blackboard_UIManager.SettingPauseMenuCanvas.SetActive(true);
        singleton.GameManager.ChangeGameState(GameState.SettingPause);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.SettingPauseMenuCanvas.SetActive(false);
        base.OnExit();
    }
}