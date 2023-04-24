using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Vida;
    public Slider LifeBar;

    // Start is called before the first frame update
    void Start()
    {
        Vida = 1.00f;
    }

    // Update is called once per frame
    void Update()
    {
        LifeBar.value = Vida;
        if (Vida == 0.00f)
        {
            die();
        }
    }
    public void TakeDamage()
    {
        Vida -= 0.10f;
    }
    private void die()
    {

    }
}
