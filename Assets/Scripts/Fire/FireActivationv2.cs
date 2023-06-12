using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireActivationv2 : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public List<FireBehavior> _fires = new List<FireBehavior>();
    public List<ExplosionBehavior> _explosions = new List<ExplosionBehavior>();
    bool playerIn;

    private void Awake()
    {
        foreach (FireBehavior f in _fires)
        {
            f.enabled = false;
        }
        foreach (ExplosionBehavior o in _explosions)
        {
            o.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;
        playerIn = false;
        //MyCollisions();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (FireBehavior f in _fires)
            {
                f.enabled = true;
            }
            foreach (ExplosionBehavior o in _explosions)
            {
                o.enabled = true;
            }
        }
    }
}
