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
    bool Dead;
    [SerializeField] private float burnIndicatorTime;

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
        blackboardUI.YouDiedImage.color = new Color(1f, 1f, 1f, 0f);
        inputPlayer.enabled = true;
        playerMovement.enabled = true;
        Dead = false;
    }
    public void TakeDamage()
    {
        if (Dead) return;
        if(!immortal) Vida -= 0.10f; isTakingDamage = true;
        StartCoroutine(Becomeinvulnerable());
        blackboardUI.SetLifeBar(Vida);
        //StopAllCoroutines();
        StartCoroutine(ShowBurnIndicator());
            
        if (Vida <= 0.00f && Dead == false)
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
        StartCoroutine(blackboardUI.FadeIN());
        StartCoroutine(Respawn());
    }
    
}
