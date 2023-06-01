using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealth PH;
    private bool OnDamageZone;
    private bool Valve;
    private float Timer;

    // Start is called before the first frame update
    void Start()
    {
        OnDamageZone = false;
        Valve = true;
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else if(OnDamageZone && !PH.isTakingDamage)
        {
            PH.TakeDamage();
            Timer = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag ==  "Player")
        {
            if (Valve)
            {
                Valve = false;
                Timer = 0;
            }
            OnDamageZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            OnDamageZone = false;
    }
}
