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
        singleton = Singleton.Instance;
        singleton.GameManager.StartLevelPreview();
        singleton.UIManager.blackboard_UIManager.LevelPreviewCanvas.SetActive(true);
        base.OnEnter();
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