using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePropagationV2 : MonoBehaviour
{

    public GameObject[] highFire;
    public GameObject[] midFire;
    public GameObject[] lowFire;
    public GameObject nearFire;
    public GameObject sonFire;
    float distance;
    float nearDistance = 3f;

    bool created = false;

    public bool firstGrade;
    public bool secondGrade;
    public bool thirdGrade;

    public int rndPercentage1;
    public int rndPercentage2;

    float fireHP = 100f;
    float timeToExplote;

    // Start is called before the first frame update
    void Start()
    {
        //firePrefab = Resources.Load("Prefabs/Firee") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateFireProp();
        fireHP -= Time.deltaTime;
    }

    public void CalculateFireProp()
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
                StartCoroutine(Instantiations());
            }
        }
        for (int i = 0; i < midFire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, midFire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = midFire[i];
                StartCoroutine(Instantiations());
            }
        }
        for (int i = 0; i < lowFire.Length; i++)
        {
            distance = Vector3.Distance(transform.position, lowFire[i].transform.position);

            if (distance < nearDistance)
            {
                nearFire = lowFire[i];
                StartCoroutine(Instantiations());
            }
        }
    }

    public IEnumerator Instantiations()
    {
        if (nearFire.GetComponent<FirePropagation>() == null)
        {
            nearFire.AddComponent<FirePropagation>();
        }

        if (nearFire.transform.tag == "HighF")
        {
            timeToExplote = Random.Range(.5f, 1f);
            if (nearFire.GetComponent<Explosion>() == null)
                nearFire.AddComponent<Explosion>();
        }
        else if (nearFire.transform.tag == "MidF")
        {
            timeToExplote = Random.Range(2f, 3f);
        }
        else if (nearFire.transform.tag == "LowF")
        {
            timeToExplote = Random.Range(4f, 5f);
        }

        yield return new WaitForSeconds(timeToExplote);

        sonFire = nearFire.transform.GetChild(0).gameObject;
        nearFire.transform.tag = "Burning";
        sonFire.SetActive(true);

        if (nearFire.GetComponent<Explosion>() == null && nearFire.transform.gameObject.tag == "HighF")
            nearFire.AddComponent<Explosion>();

        yield return new WaitForSeconds(1f);
    }


}
