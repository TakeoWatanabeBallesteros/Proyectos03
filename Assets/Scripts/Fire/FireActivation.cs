using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireActivation : MonoBehaviour
{
    bool m_Started;
    public LayerMask m_LayerMask;
    [SerializeField] List<FirePropagation> _fires = new List<FirePropagation>();
    bool playerIn;
    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;
        playerIn = false;
        MyCollisions();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerIn)
        {
            foreach (FirePropagation f in _fires)
            {
                f.enabled = true;
            }
        }

        else if (!playerIn)
        {
            foreach (FirePropagation f in _fires)
            {
                f.enabled = false;
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
            _fires.Add(hitcolliders[i + 1].gameObject.GetComponent<FirePropagation>());
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
