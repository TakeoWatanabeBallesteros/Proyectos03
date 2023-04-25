using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]private int SavedKids = 0;
    [SerializeField] private GameObject[] Kids;
    [SerializeField] private int TotalKids = 0;
    public TMP_Text NumberOfkids;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;
    }

    // Update is called once per frame
    void Update()
    {
        NumberOfkids.text = ("Kids saved: " + SavedKids + "/" + TotalKids);
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
