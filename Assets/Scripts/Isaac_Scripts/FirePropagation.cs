using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePropagation : MonoBehaviour
{

    public GameObject[] highFire;
    public GameObject[] midFire;
    public GameObject[] lowFire;
    public GameObject nearFire;
    public GameObject sonFire;
    GameObject firePrefab;
    float distance;
    public float nearDistance = 5f;

    bool created = false;

    public bool firstGrade;
    public bool secondGrade;
    public bool thirdGrade;

    int rndPercentage1;
    int rndPercentage2; 

    float fireHP = 100f;

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
        fireHP -= Time.deltaTime;
        Debug.Log(rndPercentage2);
    }


    void CalculateFireProp()
    {
        highFire = GameObject.FindGameObjectsWithTag("HighF");
        midFire = GameObject.FindGameObjectsWithTag("MidF");
        lowFire = GameObject.FindGameObjectsWithTag("LowF");

        for (int i = 0; i < highFire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, highFire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = highFire[i];
                nearDistance = distance;                
               
                if (nearFire.transform.tag == "HighF")
                {
                    StartCoroutine(Instantiations());
                }
            }
        }
        for (int i = 0; i < midFire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, midFire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = midFire[i];
                nearDistance = distance;
                StartCoroutine(ExpansionWithPercentages());

                if (nearFire.transform.tag == "MidF" && rndPercentage1 <= 70f)
                {
                    StartCoroutine(Instantiations());
                }                
            }
        }
        for (int i = 0; i < lowFire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, lowFire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = lowFire[i];
                nearDistance = distance;
                StartCoroutine(ExpansionWithPercentages());

                if (nearFire.transform.tag == "LowF" && rndPercentage2 <= 40f)
                {
                    StartCoroutine(Instantiations());
                }
                else
                {
                    Debug.Log("KeepWaiting");
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

    IEnumerator ExpansionWithPercentages()
    {
        yield return new WaitForSeconds(.1f);
        rndPercentage1 = Random.Range(0, 101);
        rndPercentage2 = Random.Range(0, 101);
    }
      

}
