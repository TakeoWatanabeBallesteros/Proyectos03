using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirePropagationV2 : MonoBehaviour
{
    public List<FirePropagationV2> allFires;
    
    //public GameObject fire;
    float nearDistance = 10f;
        
    public int highPercentage;
    public int lowPercentage;

    float fireHP = 100f;
    float timeToExplote;

    public bool onFire = false;

    public FireType fireType;

    float timer;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        //firePrefab = Resources.Load("Prefabs/Firee") as GameObject;
        allFires = FindObjectsOfType<FirePropagationV2>().ToList<FirePropagationV2>();
        allFires.RemoveAll(item => item.onFire == true);
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = delay;
            CalculateFireProp();
        }

        //fireHP -= Time.deltaTime;
        
    }

    public void CalculateFireProp()
    {     
        foreach (var x in allFires)
        {
            float distance = Vector3.Distance(transform.position, x.transform.position);

            if (distance < nearDistance && onFire)
            {
                if (x.fireType == FireType.HighFlammability && Random.Range(1,101) < highPercentage)
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    onFire = true;
                    allFires.Remove(x);
                    break;
                }
                else if(x.fireType == FireType.LowFlammability && Random.Range(1, 101) < lowPercentage)
                {
                    x.transform.GetChild(0).gameObject.SetActive(true);
                    onFire = true;
                    allFires.Remove(x);
                    break;
                }
            }
        }          

    }    

}

public enum FireType
{    
    Explosive,
    HighFlammability,    
    LowFlammability
}
