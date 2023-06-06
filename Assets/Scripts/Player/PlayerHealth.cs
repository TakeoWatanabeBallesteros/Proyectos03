using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private bool canDamaged;
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
    public float damageCoolDown;
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
        canDamaged = false;
    }
    
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4.5f);
        transform.position = initialPos;

        yield return new WaitForSeconds(.5f);
        Vida = 1;
        blackboardUI.SetLifeBar(Vida);
        blackboardUI.YouDiedImage.color = new Color(1f, 1f, 1f, 0f);
        inputPlayer.enabled = true;
        playerMovement.enabled = true;
        Dead = false;
    }
    public void TakeDamage()
    {
        if (Dead) return;
        if(!canDamaged)
        {
            Vida -= 0.10f;
            StartCoroutine(DamageCooldown());
            blackboardUI.SetLifeBar(Vida);
            StartCoroutine(ShowBurnIndicator());
        }
            
        if (Vida <= 0.00f)
        {
            Die();
        }
    }

    private IEnumerator ShowBurnIndicator()
    {
        blackboardUI.Fire.SetActive(true);
        yield return new WaitForSeconds(burnIndicatorTime);
        blackboardUI.Fire.SetActive(false);
    }

    private IEnumerator DamageCooldown()
    {
        canDamaged = true;
        yield return new WaitForSeconds(damageCoolDown);
        canDamaged = false;
    }

    private void Die()
    {
        Dead = true;
        inputPlayer.enabled = false;
        playerMovement.Stop();
        StartCoroutine(blackboardUI.FadeIN());
        StartCoroutine(Respawn());
    }
    
}
