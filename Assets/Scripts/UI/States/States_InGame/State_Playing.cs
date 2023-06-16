using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class State_Playing : StateBase
{
    private Singleton singleton;

    public State_Playing() : base(needsExitTime: false)
    {
        singleton = Singleton.Instance;
        singleton.UIManager._inGameMusic.start();
        singleton.UIManager._inGameMusic.release();
        singleton.UIManager._inGameMusic.setPaused(true);
    }
    
    public override void OnEnter()
    {
        Singleton.Instance.GameManager.ChangeGameState(GameState.Playing);
        singleton.UIManager.blackboard_UIManager.InGameCanvas.SetActive(true);
        singleton.UIManager._inGameMusic.setPaused(false);
        GameManager.OnUnpause();
        base.OnEnter();
    }
    
    public override void OnLogic()
    {
        base.OnLogic();
    }

    public override void OnExit()
    {
        singleton.UIManager.blackboard_UIManager.InGameCanvas.SetActive(false);
        singleton.UIManager._inGameMusic.setPaused(true);
        base.OnExit();
    }
}