using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Serialization;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private EventReference playerSteps;
    [SerializeField] private EventReference playerFall;
    [SerializeField] private EventReference kidThrow;
    [SerializeField] private EventReference kidPickUp;
    [SerializeField] private PlayerMovementController playerMovementController;

    public void PlayerFall() {
        RuntimeManager.PlayOneShot(playerFall);
        Instantiate(walkingParticles, transform);
    }
    
    public void CanWalk() {
        FindObjectOfType<LevelTimer>().UnpauseTimer();
        playerMovementController.canMove = true;
    }

    public void OnStep() {
        Instantiate(walkingParticles, transform);
        RuntimeManager.PlayOneShot(playerSteps);
    }

    public void Throw() {
        RuntimeManager.PlayOneShot(kidThrow);
    }

    public void Pickup() {
        RuntimeManager.PlayOneShot(kidPickUp);
    }
}
