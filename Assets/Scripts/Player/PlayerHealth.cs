using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private bool canBeDamaged;
    Vector3 initialPos;
    [SerializeField] private PlayerInputController input;
    [SerializeField] private PlayerMovementController playerMovement;
    public bool Dead;
    [SerializeField] private float burnIndicatorTime;
    private Blackboard_UIManager blackboardUI; 
    public float damageCoolDown;
    public GameObject PlayerRender;
    public GameObject DustPile;

    private PointsBehavior pointsManager;

    public float _maxHealth;
    
    public float health { get; set; }
    public float maxHealth => _maxHealth;

    public Vector3 position => transform.position;

    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private LevelTimer levelTimer;

    [SerializeField] private EventReference damageSound;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        Dead = false;
        health = maxHealth;
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        blackboardUI.SetLifeBar(health);
        pointsManager = Singleton.Instance.PointsManager;
        canBeDamaged = true;
    }

    private void StartRespawn(InputAction.CallbackContext ctx) => StartCoroutine(Respawn());
    
    private IEnumerator Respawn()
    {
        input.RemoveSpaceFunction(StartRespawn);
        transform.position = initialPos;
        health = maxHealth;
        blackboardUI.SetLifeBar(health);
        StartCoroutine(blackboardUI.FadeOut());
        blackboardUI.FireHandle.SetActive(false);
        Dead = false;
        yield return new WaitForSeconds(1f);
        levelTimer.UnpauseTimer();
        PlayerRender.SetActive(true);
        animatorController.StartAnim();
    }


    public void TakeDamage(float damage)
    {
        if (Dead || !canBeDamaged) return;
        
        health = Mathf.Clamp(health -= damage, 0, maxHealth);
        blackboardUI.SetLifeBar(health);
        RuntimeManager.PlayOneShot(damageSound);
        pointsManager.RemovePointsGettingBurned();
        StartCoroutine(DamageCooldown());
        StartCoroutine(ShowBurnIndicator());

        if (health == 0) Die();
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

    private void Die() //TODO: Do in Game Manager
    {
        Dead = true;
        input.AddSpaceFunction(StartRespawn);
        pointsManager.RemovePointsDead();
        playerMovement.Stop();
        levelTimer.PauseTimer();
        StartCoroutine(blackboardUI.FadeIN(blackboardUI.YouDiedImage));
        var ashes = Instantiate(DustPile, transform.position, Quaternion.identity);
        Destroy(ashes,4f);
        PlayerRender.SetActive(false);
    }
    
}
