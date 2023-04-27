using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    Vector3 initialPos;
    [SerializeField] private float Vida;
    InputPlayerController inputPlayer;
    MovementPlayerController playerMovement;
    PickupKid Kid;
    public Slider LifeBar;
    public Image Fire;
    public Image YouDied;
    bool Dead;
    float Timer;
    float Alfa;
    GameController GM;

    // Start is called before the first frame update
    void Start()
    {
        Kid = GetComponent<PickupKid>();
        GM = GameObject.Find("GameController").GetComponent<GameController>();
        inputPlayer = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        initialPos = transform.position;
        Dead = false;
        Fire.color = new Color(1f, 1f, 1f, 0f);
        Timer = 0.5f;
        Vida = 1.00f;
    }

    // Update is called once per frame
    void Update()
    {
        LifeBar.value = Vida;
        if (Vida <= 0.00f && Dead == false)
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
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4.5f);
        transform.position = initialPos;
        yield return new WaitForSeconds(.5f);
        Vida = 1;
        YouDied.color = new Color(1f, 1f, 1f, 0f);
        inputPlayer.enabled = true;
        playerMovement.enabled = true;
        Dead = false;
        GM.AddTime(999f);
    }
    public void TakeDamage()
    {
        if (!Dead)
        {
            if (Kid.HasKid())
                IntantDeath();
            else
            {
                Vida -= 0.10f;
                Timer = 0.5f;
                Fire.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    private void die()
    {
        Dead = true;
        inputPlayer.enabled = false;
        playerMovement.enabled = false;
        StartCoroutine(FadeIN(YouDied));
        StartCoroutine(Respawn());
    }
    IEnumerator FadeIN(Image image)
    {
        Alfa += .1f;
        image.color = new Color(1f, 1f, 1f, Alfa);
        yield return new WaitForSeconds(.1f);
        if (Alfa < 1)
        {
            StartCoroutine(FadeIN(image));
        }
    }
    public void IntantDeath()
    {
        Vida = 0;
    }
}
