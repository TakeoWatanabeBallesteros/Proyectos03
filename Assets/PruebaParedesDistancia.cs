using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaParedesDistancia : MonoBehaviour
{
    public MeshRenderer mat;
    public float Distancia;
    private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 D = transform.position - Player.position;
        Distancia = -D.magnitude;
        mat.material.SetFloat("_DisapearR", Distancia + 2);
    }
}
