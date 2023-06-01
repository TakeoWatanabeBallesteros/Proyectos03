using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public GameObject mykid;
    ItemManager Kids;
    // Start is called before the first frame update
    void Start()
    {
        Kids = FindObjectOfType<ItemManager>();
        mykid.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kid")
        {
            Destroy(other.transform.parent.gameObject);
            mykid.SetActive(true);
            Kids.AddChild();
        }
    }
}
