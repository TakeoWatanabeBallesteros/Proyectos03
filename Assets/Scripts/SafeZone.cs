using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SafeZone : MonoBehaviour
{
    ItemManager itemManager;

    private int counter;

    public List<GameObject> childsGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        counter = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kid"))
        {
            Destroy(other.transform.parent.gameObject);
            if(counter < 5) childsGameObjects[counter++].SetActive(true);
            itemManager.AddChild();
        }
    }
}
