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

    void Start()
    {
        UI_Blackboard = Singleton.Instance.UIManager.blackboard_UIManager;
        SavedKids = 0;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
        Collected = 0;
        UI_Blackboard.SetColectables(Collected, TotalCollectables);

    }

    // Update is called once per frame
    private void Update()
    {
        /*
        if(TimerEnSegundos>0 && SavedKids >= TotalKids)
        {
            Win();
        }
        */

    }
    public int GetTotalKids()
    {
        return TotalKids;
    }
    public int GetSavedKids()
    {
        return TotalKids;
    }
    public void AddChild()
    {
        SavedKids++;
        UI_Blackboard.SetKids(SavedKids, TotalKids);
    }

    public void AddCollectable()
    {
        Collected++;
        UI_Blackboard.SetColectables(Collected, TotalCollectables);
    }

}
