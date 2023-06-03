using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireActivationv2 : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public List<FirePropagation2> _fires = new List<FirePropagation2>();
    public List<ObjectsExplosionv2> _explosions = new List<ObjectsExplosionv2>();
    public List<MaterialLerping> _materialLerping = new List<MaterialLerping>();
    bool playerIn;
    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;
        playerIn = false;
        //MyCollisions();
    }

    // Update is called once per frame
    void Update()
    {       

        if (playerIn == true)
        {
            foreach (FirePropagation2 f in _fires)
            {
                f.enabled = true;
            }
            foreach (ObjectsExplosionv2 o in _explosions)
            {
                o.enabled = true;
            }
            foreach (MaterialLerping m in _materialLerping)
            {
                m.enabled = true;
            }
        }
        else
        {
            foreach (FirePropagation2 f in _fires)
            {
                f.enabled = false;
            }
            foreach (ObjectsExplosionv2 o in _explosions)
            {
                o.enabled = false;
            }
            foreach (MaterialLerping m in _materialLerping)
            {
                m.canLerpMaterials = false;
                m.enabled = false;
            }
        }
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
            playerIn = true;
        }
    }
}
