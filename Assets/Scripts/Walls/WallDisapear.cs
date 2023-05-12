using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisapear : MonoBehaviour
{
    public GameObject[] Paredes;
    private float WallOpacity = 1;
    public float OcludedHeight;
    // Start is called before the first frame update

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
        WallOpacity += Dir * .02f;
        for (int i = 0; i < Paredes.Length; i++)
        {
            Paredes[i].GetComponent<MeshRenderer>().material.SetFloat("_Alpha", WallOpacity);
        }
        yield return new WaitForSeconds(.01f);

        if (Dir == 1 && WallOpacity < 0)
        {
            StartCoroutine(OcludeWall(Dir));
        }
        if (Dir == -1 && WallOpacity > 1)
        {
            StartCoroutine(OcludeWall(Dir));
        }
    }
}
