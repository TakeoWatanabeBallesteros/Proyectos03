using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiresRoomManager : MonoBehaviour
{
    bool m_Started;
    public List<FireBehavior> _fires;
    // Start is called before the first frame update
    void Start()
    {
        _fires = GetComponentsInChildren<FireBehavior>().ToList();
        foreach (FireBehavior f in _fires)
        {
            f.active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {       
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (FireBehavior f in _fires)
            {
                f.active = true;
            }
        }
    }
}
