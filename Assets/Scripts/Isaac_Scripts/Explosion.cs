using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float Timer = 3.5f;
    float explosionTimer = 7f;

    float explosionRadius = 10;
    float maxDistance = 30;
    LayerMask _layerMask = ~0; //Para pillar todas las layers

    float nearFireRadius = 5f;
    float midFireRadius = 10f;
    float farFireRadius = 15f;

    public List<GameObject> goList;

    float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        explosionTimer -= Time.deltaTime;   

        if (Timer <= 0)
        {
            Timer = 0;

            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            FindGameObjectsInLayer(6);
            ApplyFirePropagation();
        }

        if (explosionTimer <= 0)
        {
            explosionTimer = 0;
            gameObject.GetComponent<FirePropagation>().enabled = false;
        }

    }

    GameObject[] FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer && goArray[i].transform.tag != "Burning")
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    void ApplyFirePropagation()
    {
        for(int i=0; i < goList.Count; i++)
        {
            distance = Vector3.Distance(goList[i].transform.position, transform.position);
            int rndValue = Random.Range(1, 10);

            if(explosionTimer > 0)
            {
                if (distance <= nearFireRadius)
                {
                    StartCoroutine(goList[i].transform.gameObject.GetComponent<FirePropagationV2>().Instantiations());
                    //Debug.Log("somethingnear");
                }

                if (distance <= midFireRadius && rndValue < 5)
                {
                    StartCoroutine(goList[i].transform.gameObject.GetComponent<FirePropagationV2>().Instantiations());
                    //Debug.Log("somethingclose");
                }

                if (distance <= farFireRadius && rndValue < 3)
                {
                    StartCoroutine(goList[i].transform.gameObject.GetComponent<FirePropagationV2>().Instantiations());
                    //Debug.Log("somethingfar");
                }
            }
            
        }
    }
}

