using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private bool canBeDamaged;
    Vector3 initialPos;
    InputPlayerController inputPlayer;
    MovementPlayerController playerMovement;
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
    
    private PlayerControls controls;

    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private LevelTimer levelTimer;

    // Start is called before the first frame update
    void Start()
    {
        inputPlayer = GetComponent<InputPlayerController>();
        playerMovement = GetComponent<MovementPlayerController>();
        initialPos = transform.position;
        Dead = false;
        health = maxHealth;
        blackboardUI = Singleton.Instance.UIManager.blackboard_UIManager;
        pointsManager = Singleton.Instance.PointsManager;
        canBeDamaged = true;
        controls = controls ?? new PlayerControls();
        controls.Enable();
    }
    
    IEnumerator Respawn()
    {
        do {
            yield return null;
        } while (!controls.Player.Restart.triggered);
        
        transform.position = initialPos;
        health = maxHealth;
        blackboardUI.SetLifeBar(health);
        StartCoroutine(blackboardUI.FadeOut());
        blackboardUI.FireHandle.SetActive(false);
        Dead = false;
        yield return new WaitForSeconds(1f);
        inputPlayer.enabled = true;
        levelTimer.UnpauseTimer();
        PlayerRender.SetActive(true);
        animatorController.StartAnim();
    }


    public void TakeDamage(float damage)
    {
        if (Dead) return;
        if(canBeDamaged)
        {
            health = Mathf.Clamp(health -= damage, 0, maxHealth);
            StartCoroutine(DamageCooldown());
            blackboardUI.SetLifeBar(health);
            StartCoroutine(ShowBurnIndicator());
            pointsManager.RemovePointsGettingBurned();
        }
            
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

    private void Die()
    {
        Dead = true;
        pointsManager.RemovePointsDead();
        inputPlayer.enabled = false;
        playerMovement.Stop();
        levelTimer.PauseTimer();
        StartCoroutine(blackboardUI.FadeIN());
        StartCoroutine(Respawn());
        //activar polvo
        GameObject polvo = Instantiate(DustPile);
        polvo.transform.position = transform.position;
        Destroy(polvo,4f);
        //desactivar modelo
        PlayerRender.SetActive(false);
    }
    
}
