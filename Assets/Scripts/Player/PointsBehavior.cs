using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsBehavior : MonoBehaviour
{
    public int ExtinguishFirePoints;
    
    public static int Points;

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
    public void AddPoints(int points) { Points += points; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddPointsCombo(int points) { Points += points * Combo; blackboardUI.SetPoints( Points, pointsToWin); }
    public void AddCombo() => Combo++; //TO DO: Add combos to fire propagation
    public void ResetCombo() => Combo = 1; //TO DO: Add reset combo to gold water when not shooting
/*  
    public static void AddPointsSaveZone(int attempts, int distance) => Points += 1000 + (200 / attempts) + 10 * distance;
    public static void AddPointsSaveZone() => Points += 1000;
    public static void AddPointsExplosion() => Points += 750;
    public static void AddPointsCollectable() => Points += 500;
    public static void AddPointsSeconds(int seconds) => Points += 50 * seconds;
*/
    public void ResetPoints() => Points = 0;
}
