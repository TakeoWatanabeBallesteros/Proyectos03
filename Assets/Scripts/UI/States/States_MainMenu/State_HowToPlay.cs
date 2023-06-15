using FSM;

public class State_HowToPlay : StateBase
{
    private Singleton singleton;
    
    public State_HowToPlay() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        singleton = Singleton.Instance;
        Singleton.Instance.GameManager.ChangeGameState(GameState.HowToPlay);
        singleton.UIManager.blackboard_UIManager.HowToPlayCanvas.SetActive(true);
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.HowToPlayCanvas.SetActive(false);
        base.OnExit();
    }
}