using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsBehavior : MonoBehaviour
{
    
    public static int Points;

    [Header("Points by action")]
    public int SafeZonePoints;
    public int ExplosionPoints;
    public int CollectablePoints;
    public int PointsPerSecond;
    public int ExtinguishFirePoints;

    [Header("Win")]
    public static int Combo;

    public int pointsToWin;

    private Blackboard_UIManager blackboardUI;
    
    // Start is called before the first frame update
    void Start()
    {
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        Points = 0;
        blackboardUI.SetPoints(Points, pointsToWin);
    }

    // Update is called once per frame
    void Update()
    {
        //blackboardUI.SetPoints(Points, pointsToWin);
    }
    public void AddPointsSafeZone() { Points += SafeZonePoints; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddPointsExplosion() { Points += ExplosionPoints; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddPointsCollectable() { Points += CollectablePoints; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddPointsSeconds() { Points += PointsPerSecond; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddPointsCombo() { Points += ExtinguishFirePoints * Combo; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddCombo() => Combo++; 
    public void ResetCombo() => Combo = 1;
/*  

*/
    public void ResetPoints() => Points = 0;
}
