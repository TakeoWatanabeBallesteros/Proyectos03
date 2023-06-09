using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [FormerlySerializedAs("canDamaged")] [SerializeField] private bool canBeDamaged;
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

    private PointsBehavior pointsManager;

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
        pointsManager = Singleton.Instance.PointsManager;
        canBeDamaged = true;
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
        if(canBeDamaged)
        {
            Vida -= 0.10f;
            Vida = Mathf.Clamp(Vida, 0, 1);
            StartCoroutine(DamageCooldown());
            blackboardUI.SetLifeBar(Vida);
            StartCoroutine(ShowBurnIndicator());
            pointsManager.RemovePointsGettingBurned();
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
        canBeDamaged = false;
        yield return new WaitForSeconds(damageCoolDown);
        canBeDamaged = true;
    }

    private void Die()
    {
        Dead = true;
        pointsManager.RemovePointsDead();
        inputPlayer.enabled = false;
        playerMovement.Stop();
        StartCoroutine(blackboardUI.FadeIN());
        StartCoroutine(Respawn());
    }
    
}
