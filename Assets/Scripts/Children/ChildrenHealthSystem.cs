using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChildrenHealthSystem : MonoBehaviour
{

    [SerializeField] float childHP = 100;
    bool isInmortal = false;
    float timer;
    public float delay = 1;
    public bool isGettingBurned;
    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
        isGettingBurned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGettingBurned)
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
        isGettingBurned = true;
        if (!isInmortal)
        {
            childHP -= 10f;
        }  
        if (childHP <= 0)
        {
            Debug.Log("Estoy muerto vivo"); //Do some particles or whatever animation and then disable kid
        }
    }

    public void StopBeingBurned()
    {
        isGettingBurned = false;
        timer = 0;
    }

}
