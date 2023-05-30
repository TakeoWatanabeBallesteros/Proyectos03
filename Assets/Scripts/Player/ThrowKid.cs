using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowKid : MonoBehaviour
{
    public GameObject MyBalls;
    InputPlayerController Input;
    public Transform Hombro;
    public float Fuerza = 1f;
    const float MaxFuerza = 900f;
    public bool Holding = false;
    public float FuerzaPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        Input = GetComponent<InputPlayerController>();
        Fuerza = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Holding && !Input.secondaryShoot)
        {
            Debug.Log("Spawn");
            var ball = Instantiate(MyBalls);
            ball.transform.position = Hombro.transform.position;
            ball.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * Fuerza, ForceMode.Impulse);
        }
        Holding = Input.secondaryShoot;
        if (Holding) Fuerza = Mathf.Clamp(Fuerza += FuerzaPerSecond * Time.deltaTime, 0, MaxFuerza);
        
        else Fuerza = 0;


    }
}
