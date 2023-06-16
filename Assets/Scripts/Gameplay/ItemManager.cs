using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    Blackboard_UIManager UI_Blackboard;
    [SerializeField] private int SavedKids = 0;
    [SerializeField] private int TotalKids = 0;

    [SerializeField] private int Collected = 0;
    [SerializeField] private int TotalCollectables = 0;

    private PlayerInputController _playerInput;
    private PlayerMovementController move;
    private FSM_UIManager uiManager;
    private PointsBehavior pointsManager;
    private FinalScreenManager finalScreenManager;
    
    [ContextMenu("Do Something")]
    void DoSomething()
    {
        uiManager.uiManager_FSM.Trigger("Playing-Win");
    }
    
    void Start()
    {
        UI_Blackboard = Singleton.Instance.UIManager.blackboard_UIManager;
        uiManager = Singleton.Instance.UIManager;
        pointsManager = Singleton.Instance.PointsManager;
        _playerInput = Singleton.Instance.Player.GetComponent<PlayerInputController>();
        move = Singleton.Instance.Player.GetComponent<PlayerMovementController>();
        SavedKids = 0;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        Collected = 0;
        UI_Blackboard.SetCollectables(Collected, TotalCollectables);

    }

    public void AddChild()
    {
        SavedKids++;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        pointsManager.AddPointsSafeZone();
        if(SavedKids == TotalKids) {
            uiManager.uiManager_FSM.Trigger("Playing-Win");
        }
    }

    public void AddCollectable()
    {
        Collected++;
        UI_Blackboard.SetCollectables(Collected, TotalCollectables);
        pointsManager.AddPointsCollectable();
    }

    public void DeadChild()
    {
        TotalKids--;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        pointsManager.RemovePointsChildBurned();
        if(SavedKids == TotalKids) {
            uiManager.uiManager_FSM.Trigger("Playing-Win");
        }
    }

    public void DeadCollectable()
    {
        TotalCollectables--;
        UI_Blackboard.SetCollectables(Collected, TotalCollectables);
        pointsManager.RemovePointsCollectable();
    }
}
