using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaParedesDistancia : MonoBehaviour
{
    public GameObject[] Paredes;
    public float WallHeight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WallHeight = Paredes[0].GetComponent<MeshRenderer>().material.GetFloat("_DisapearR");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(OcludeWall(1));

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(OcludeWall(-1));
            
        }
    }
    private IEnumerator OcludeWall(int Dir)
    {
        WallHeight += Dir * .2f;
        for (int i = 0; i < Paredes.Length; i++)
        {
            Paredes[i].GetComponent<MeshRenderer>().material.SetFloat("_DisapearR", WallHeight);
        }
        yield return new WaitForSeconds(.02f);

        if (Dir == 1 && WallHeight < -1)
        {
            StartCoroutine(OcludeWall(Dir));
        }
        if (Dir == -1 && WallHeight > -10)
        {
            StartCoroutine(OcludeWall(Dir));
        }
    }
}
