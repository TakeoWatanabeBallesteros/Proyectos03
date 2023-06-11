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
    bool Dead;
    [SerializeField] private float burnIndicatorTime;
    private Blackboard_UIManager blackboardUI; 
    public float damageCoolDown;

    private PointsBehavior pointsManager;

    public float _maxHealth;
    
    public float health { get; set; }
    public float maxHealth => _maxHealth;

    public Vector3 position => transform.position;

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
    }
    
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4.5f);
        transform.position = initialPos;

        yield return new WaitForSeconds(.5f);
        health = maxHealth;
        blackboardUI.SetLifeBar(health);
        blackboardUI.YouDiedImage.color = new Color(1f, 1f, 1f, 0f);
        blackboardUI.FireHandle.SetActive(false);
        inputPlayer.enabled = true;
        playerMovement.enabled = true;
        Dead = false;
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
        StartCoroutine(blackboardUI.FadeIN());
        StartCoroutine(Respawn());
    }
    
}
