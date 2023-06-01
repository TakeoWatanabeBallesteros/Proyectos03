using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsBehavior : MonoBehaviour
{
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
        blackboardUI.SetPoints(Points, pointsToWin);
    }

    public static void IncreaseCombo() => Combo = Combo < 10 ? Combo + 1 : Combo;
    public static void ResetCombo() => Combo = 1;
    public static void AddPointsFire() => Points += 10 * Combo;
    public static void AddPointsSaveZone(int attempts, int distance) => Points += 1000 + (200 / attempts) + 10 * distance;
    public static void AddPointsSaveZone() => Points += 1000;
    public static void AddPointsExplosion() => Points += 750;
    public static void AddPointsCollectable() => Points += 500;
    public static void AddPointsSeconds(int seconds) => Points += 50 * seconds;
    public static void ResetPints() => Points = 0;
}
