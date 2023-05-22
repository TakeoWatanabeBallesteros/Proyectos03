using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireActivation : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    public List<FirePropagation> _fires = new List<FirePropagation>();
    public List<ObjectsExplosion> _explosions = new List<ObjectsExplosion>();
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
            foreach (FirePropagation f in _fires)
            {
                f.enabled = true;
            }
            foreach (ObjectsExplosion o in _explosions)
            {
                o.enabled = true;
                o.preExplosion = true;
            }
        }
        else
        {
            foreach (FirePropagation f in _fires)
            {
                f.StopAllCoroutines();
                f.enabled = false;
            }
            foreach (ObjectsExplosion o in _explosions)
            {
                o.enabled = false;
                o.preExplosion = false;
            }
        }
    }


    void MyCollisions()
    {
        Collider[] hitcolliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        int i = 0;
        while (i < hitcolliders.Length)
        {
            i++;
            _fires.Add(hitcolliders[i].gameObject.GetComponent<FirePropagation>());
            _explosions.Add(hitcolliders[i].gameObject.GetComponent<ObjectsExplosion>());
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
        if (other.tag == "Player")
        {
            playerIn = true;
            Debug.Log("Ha entrado el player");
        }
    }
}
