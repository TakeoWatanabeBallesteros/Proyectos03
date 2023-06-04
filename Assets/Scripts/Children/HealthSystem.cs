using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] float childHP = 100;
    bool isInmortal = false;
    float timer;
    public float delay = 1;
    public bool startTimer;
    public bool isGettingDamaged;
    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
        isGettingDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (childHP <= 0)
        {
            Debug.Log("Estoy muerto vivo");
        }
        if (startTimer && isGettingDamaged)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                isInmortal = true;
            }
            else
            {
                timer = delay;
                isInmortal = false;
                childHP -= 10;
            }
        }

    }

    public void TakeDamage()
    {
        startTimer = true;
        isGettingDamaged = true;
        if (!isInmortal)
        {
            childHP -= 10f;
        }        
    }

    public void RestartTimer()
    {
        startTimer = false;
        isGettingDamaged = false;
        timer = 0;
    }

}
