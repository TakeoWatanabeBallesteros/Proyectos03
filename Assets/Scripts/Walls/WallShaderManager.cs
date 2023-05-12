using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShaderManager : MonoBehaviour
{
    public GameObject[] Paredes;
    private float WallHeight;
    public float OcludedHeight;
    // Start is called before the first frame update
    void Start()
    {
        WallHeight = Paredes[0].GetComponent<MeshRenderer>().material.GetFloat("_DisapearR");

    }

    // Update is called once per frame
    void Update()
    {
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
        yield return new WaitForSeconds(.01f);

        if (Dir == 1 && WallHeight < -OcludedHeight)
        {
            StartCoroutine(OcludeWall(Dir));
        }
        if (Dir == -1 && WallHeight > -20)
        {
            StartCoroutine(OcludeWall(Dir));
        }
    }
}