using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsExplosion : MonoBehaviour
{
    // Start is called before the first frame update

    public bool doExplote = false;
    public List<FirePropagationV2> nearObjectsOnFire = new List<FirePropagationV2>();

    public float closeRange;
    public float midRange;
    public float highRange;

    public float maxRangeExplosion;

    void Start()
    {
        nearObjectsOnFire = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        nearObjectsOnFire.RemoveAll(item => item.onFire == true);        
        
        
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

        if (doExplote == true)
        {
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
                if (distance <= midRange && distance > closeRange) //si esta m�s cerca del mid range su flammability ser� el doble
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    x.onFire = true;
                    nearObjectsOnFire.Remove(x);
                    break;
                }
                else if (distance <= highRange && distance > midRange) //si est� m�s cerca del high range su flammability ser� su misma
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
