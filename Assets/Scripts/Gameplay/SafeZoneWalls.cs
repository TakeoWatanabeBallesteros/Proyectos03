using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SafeZoneWalls : MonoBehaviour
{
    ItemManager itemManager;
    private PointsBehavior pointsManager;
    public GameObject ThisChild;

    [SerializeField] private KidPickAndThrow pickAndThrow;
    
    void Start()
    {
        Physics.IgnoreLayerCollision(13, 14);
        itemManager = Singleton.Instance.ItemsManager;
        pointsManager = Singleton.Instance.PointsManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kid"))
        {
            ThisChild = other.gameObject;
            other.tag = "KidExtracted";
            SlowKidLimbs();
            StartCoroutine(Wait());
            itemManager.AddChild();
            pickAndThrow.ForgetKid();
            //pointsManager.AddPointsSafeZone();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        ThisChild.gameObject.layer = 1;
    }
    /* De momento creo que no es necasario pero lo dejo por si acaso
    */
    void SlowKidLimbs()
    {
        for (int i = 0; i< ThisChild.transform.childCount;i++)
        {
            ThisChild.transform.GetChild(i).gameObject.layer = 1;
        }
    }
}
