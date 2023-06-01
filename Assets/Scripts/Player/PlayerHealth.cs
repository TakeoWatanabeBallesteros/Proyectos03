using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private bool immortal;
    Vector3 initialPos;
    [SerializeField] private float Vida;
    InputPlayerController inputPlayer;
    MovementPlayerController playerMovement;
    PickupKid Kid;
    public GameObject Fire;
    public Image YouDied;
    bool Dead;
    [SerializeField] private float burnIndicatorTime;
    float Alfa;
    GameManager GM;
    private Blackboard_UIManager blackboardUI;
    public float invulnearabilityDuration;
    public bool isTakingDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        Kid = GetComponent<PickupKid>();
        GM = FindObjectOfType<GameManager>();
        inputPlayer = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        initialPos = transform.position;
        Dead = false;
        Vida = 1.00f;
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        immortal = false;
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
        if (Dead) return;
        if(!immortal) Vida -= 0.10f; isTakingDamage = true;
        StartCoroutine(Becomeinvulnerable());
        blackboardUI.SetLifeBar(Vida);
        //StopAllCoroutines();
        StartCoroutine(ShowBurnIndicator());
            
        if (Vida <= 0.00f && Dead == false && !immortal)
        {
            die();
        }
    }

    private IEnumerator ShowBurnIndicator()
    {
        Fire.SetActive(true);
        yield return new WaitForSeconds(burnIndicatorTime);
        isTakingDamage = false;
        Fire.SetActive(false);
    }

    IEnumerator Becomeinvulnerable()
    {
        immortal = true;
        yield return new WaitForSeconds(invulnearabilityDuration);
        immortal = false;
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
}
