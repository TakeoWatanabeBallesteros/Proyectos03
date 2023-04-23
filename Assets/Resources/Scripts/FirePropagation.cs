using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePropagation : MonoBehaviour
{

    public GameObject[] Fire;
    public GameObject nearFire;
    public GameObject sonFire;
    GameObject firePrefab;
    float distance;
    float nearDistance = 5f;

    bool created = false;

    // Start is called before the first frame update
    void Start()
    {
        firePrefab = Resources.Load("Prefabs/Firee") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFireProp();
    }


    void CalculateFireProp()
    {
        Fire = GameObject.FindGameObjectsWithTag("Inflamable");

        for (int i = 0; i < Fire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, Fire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = Fire[i];
                nearDistance = distance;

                if (nearFire.transform.tag == "Inflamable")
                {
                    StartCoroutine(Instantiations());
                }
            }
        }
    }

    IEnumerator Instantiations()
    {
        yield return new WaitForSeconds(2f);
        sonFire = nearFire.transform.GetChild(0).gameObject;     
        nearFire.transform.tag = "Burning";
        nearFire.AddComponent<FirePropagation>();
        sonFire.SetActive(true);
    }

}
