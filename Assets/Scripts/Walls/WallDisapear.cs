using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisapear : MonoBehaviour
{
    public GameObject[] Paredes;
    public float WallOpacity = 1;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Paredes.Length; i++)
        {
            Paredes[i].GetComponent<MeshRenderer>().material.SetFloat("_Opacity", WallOpacity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(OcludeWall(-1));

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(OcludeWall(1));

        }
    }
    private IEnumerator OcludeWall(int OP)
    {
        WallOpacity += .05f * OP;
        for (int i = 0; i < Paredes.Length; i++)
        {
            Paredes[i].GetComponent<MeshRenderer>().material.SetFloat("_Opacity", WallOpacity);
        }
        yield return new WaitForSeconds(.01f);

        if (OP == 1 && WallOpacity < 1)
        {
            StartCoroutine(OcludeWall(OP));
        }
        if (OP == -1 && WallOpacity > .15f)
        {
            StartCoroutine(OcludeWall(OP));
        }
    }
}
