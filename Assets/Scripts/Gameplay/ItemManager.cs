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

    private InputPlayerController input;
    private MovementPlayerController move;
    private FSM_UIManager uiManager;
    private PointsBehavior pointsManager;
    
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
        input = Singleton.Instance.Player.GetComponent<InputPlayerController>();
        move = Singleton.Instance.Player.GetComponent<MovementPlayerController>();
        SavedKids = 0;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        Collected = 0;
        UI_Blackboard.SetColectables(Collected, TotalCollectables);

    }

    public void AddChild()
    {
        SavedKids++;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        pointsManager.AddPoints(1000);
        if(SavedKids == TotalKids) {
            uiManager.uiManager_FSM.Trigger("Playing-Win");
        }
    }

    public void AddCollectable()
    {
        Collected++;
        UI_Blackboard.SetColectables(Collected, TotalCollectables);
        pointsManager.AddPoints(500);
    }

}
