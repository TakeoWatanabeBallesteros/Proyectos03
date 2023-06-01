using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsBehavior : MonoBehaviour
{
    public static int Points;

    public static int Combo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void IncreaseCombo() => Combo = Combo < 10 ? Combo + 1 : Combo;
    public static void ResetCombo() => Combo = 1;
    public static void AddPointsFire() => Points += 10 * Combo;
    public static void AddPointsSaveZone(int attempts, int distance) => Points += 1000 + (200 / attempts) + 10 * distance;
    public static void AddPointsExplosion() => Points += 750;
    public static void AddPointsCollectable() => Points += 500;
    public static void AddPointsSeconds(int seconds) => Points += 50 * seconds;
    public static void ResetPints() => Points = 0;
}
