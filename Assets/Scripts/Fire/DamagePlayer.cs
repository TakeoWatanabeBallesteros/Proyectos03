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
        PH = Singleton.Instance.Player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

        if(OnDamageZone && !PH.isTakingDamage)
        {
            PH.TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) OnDamageZone = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) OnDamageZone = false;
    }
}
