using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThrowKid : MonoBehaviour
{
    Slider ForceBar;
    public GameObject MyBalls;
    InputPlayerController Input;
    public Transform Hombro;
    public float Fuerza = 1f;
    const float MaxFuerza = 1000f;
    public bool Holding = false;
    public float FuerzaPerSecond;
    PickupKid pickupKid;
    // Start is called before the first frame update
    void Start()
    {
        pickupKid = GetComponent<PickupKid>();
        Input = GetComponent<InputPlayerController>();
        Fuerza = 0;
        ForceBar = GameObject.Find("ForceBar").GetComponent<Slider>();
        ForceBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickupKid.HasKid())
        {
            return;
        }
        else
        {
            ThrowTheKid();
        }


    }
    public void ThrowTheKid()
    {
        if (Holding && !Input.secondaryShoot && pickupKid.HasKid())
        {
            var ball = Instantiate(MyBalls);
            ball.transform.position = Hombro.transform.position;
            ball.transform.Find("Root").GetComponent<Rigidbody>().AddForce(transform.forward.normalized * Fuerza, ForceMode.Impulse);
            pickupKid.KidYeet();
        }
        Holding = Input.secondaryShoot;
        if (Holding)
        {
            ForceBar.gameObject.SetActive(true);
            Fuerza = Mathf.Clamp(Fuerza += FuerzaPerSecond * Time.deltaTime, 0, MaxFuerza);
        }
        else
        {
            ForceBar.gameObject.SetActive(false);
            Fuerza = 0;
        }
        ForceBar.value = Fuerza;
    }
}
