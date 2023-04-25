using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private PlayerHealth PH;
    private bool OnDamageZone;
    // Start is called before the first frame update
    void Start()
    {
        OnDamageZone = false;
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DamageOverTime());
        OnDamageZone = true;
    }
    private void OnTriggerExit(Collider other)
    {
        OnDamageZone = false;
    }
    IEnumerator DamageOverTime()
    {
        PH.TakeDamage();
        yield return new WaitForSeconds(1f);
        if (OnDamageZone)
        {
            StartCoroutine(DamageOverTime());
        }

    }
}
