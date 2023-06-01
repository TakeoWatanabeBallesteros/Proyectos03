using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidsManager : MonoBehaviour
{
    // Start is called before the first frame update
    Blackboard_UIManager UI_Blackboard;

    void Start()
    {
        UI_Blackboard = Singleton.Instance.UIManager.blackboard_UIManager;
        /* 
        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;

        Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        TotalCollectables = Collectables.Length;
 
        winScreen.SetActive(false);
        */
    }

    // Update is called once per frame
    private void Update()
    {
        /*
        NumberOfkids.text = ("Kids saved: " + SavedKids + "/" + TotalKids);
        NumberOfCollectables.text = ("Collectables: " + Collected + "/" + TotalCollectables);
    
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
    }

}
