using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    public bool doExplote = false;
    public bool preExplosion = false;
    public List<FirePropagationV2> nearObjectsOnFire = new List<FirePropagationV2>();

    public float closeRange;
    public float midRange;
    public float highRange;

    public float maxRangeExplosion;

    public Material mat1;
    public Material mat2;
    float duration = 1.0f;
    [SerializeField]Renderer rend;

    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);        
        rend = GetComponent<Renderer>();
        rend.material = mat1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i <= nearObjectsOnFire.Count(); i++)
        {
            if (Vector3.Distance(transform.position, nearObjectsOnFire[i].transform.position) <= maxRangeExplosion)
            {
                nearObjectsOnFire.Remove(nearObjectsOnFire[i]);
            }
        }*/

        if(preExplosion == true)
        {
            float lerp = Mathf.PingPong(Time.deltaTime, duration) / duration;
            mat1.Lerp(mat1, mat2, lerp);
            mat2.Lerp(mat2, mat1, lerp);
        }
        

        if (doExplote == true)
        {
            preExplosion = false;
            Debug.Log("calculate");
            CalculateExpansion();          

        }
    }

    void CalculateExpansion()
    {
        foreach (var x in nearObjectsOnFire)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < maxRangeExplosion && !x.onFire)
            {
                if (distance <= midRange && distance > closeRange) //si esta más cerca del mid range su flammability será el doble
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if (distance <= highRange && distance > midRange) //si está más cerca del high range su flammability será su misma
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if(distance <= closeRange) //aqui siempre se va a incendiar
                {
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
            }
        }
    }
}
