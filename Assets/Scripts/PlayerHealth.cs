using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Vida;
    public Slider LifeBar;
    public Image Fire;
    private bool PlayingEfect;
    private float Timer;
    private float ElapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        Fire.color = new Color(1f, 1f, 1f, 0f);
        Timer = 0.5f;
        Vida = 1.00f;
    }

    // Update is called once per frame
    void Update()
    {
        LifeBar.value = Vida;
        if (Vida <= 0.00f)
        {
            die();
        }

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Fire.color = new Color(1f, 1f, 1f, 0f);
        }
    }
    public void TakeDamage()
    {
        Vida -= 0.10f;
        Timer = 0.5f;
        Fire.color = new Color(1f, 1f, 1f, 1f);
    }

    private void die()
    {

    }
}
