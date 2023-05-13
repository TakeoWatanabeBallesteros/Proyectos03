using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEfectController : MonoBehaviour
{
    public ParticleSystem P1;
    public ParticleSystem P2;
    public ParticleSystem P3;
    public ParticleSystem P4;
    public GameObject[] Fuegos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Fuegos = GameObject.FindGameObjectsWithTag("Fire");
        var p1 = P1.emission;
        p1.rateOverTime = Fuegos.Length;
        var p2 = P2.emission;
        p2.rateOverTime = Fuegos.Length;
        var p3 = P3.emission;
        p3.rateOverTime = Fuegos.Length;
        var p4 = P4.emission;
        p4.rateOverTime = Fuegos.Length;
    }
}
