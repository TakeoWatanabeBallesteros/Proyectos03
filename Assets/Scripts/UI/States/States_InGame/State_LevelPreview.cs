using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class State_LevelPreview  : StateBase
{
    private Singleton singleton;

    public State_LevelPreview() : base(needsExitTime: false)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        singleton = Singleton.Instance;
        singleton.GameManager.ChangeGameState(GameState.LevelPreview);
        singleton.UIManager.blackboard_UIManager.LevelPreviewCanvas.SetActive(true);
        singleton.CameraPreviewManager.LoadPreviewCameras();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.LevelPreviewCanvas.SetActive(false);
        base.OnExit();
    }
}