using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private int SavedKids;
    private int TotalKids;
    public TMP_Text kids;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        kids.text = ("Kids saved: " + SavedKids + "/" + TotalKids);
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
