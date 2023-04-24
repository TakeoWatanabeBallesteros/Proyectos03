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
    public float nearDistance = 5f;

    bool created = false;

    public bool firstGrade;
    public bool secondGrade;
    public bool thirdGrade;

    float rndPercentage;

    // Start is called before the first frame update
    void Start()
    {
        firePrefab = Resources.Load("Prefabs/Firee") as GameObject;

        //rndPercentage = Random.Range(5f, 20f);
        
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
        if (nearFire.GetComponent<FirePropagation>() == null)
        {
            nearFire.AddComponent<FirePropagation>();
        }
        yield return new WaitForSeconds(2f);
        sonFire = nearFire.transform.GetChild(0).gameObject;     
        nearFire.transform.tag = "Burning";        
        sonFire.SetActive(true);
    }

}
