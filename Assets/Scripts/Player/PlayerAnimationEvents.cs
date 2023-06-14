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
    [SerializeField] private MovementPlayerController movementPlayerController;


    public void PlayerFall()
    {
        RuntimeManager.PlayOneShot(playerFall);
        walkingParticles.Play();
    }
    
    public void CanWalk()
    {
        FindObjectOfType<LevelTimer>().UnpauseTimer();
        movementPlayerController.canMove = true;
    }

    public void OnStep()
    {
        walkingParticles.Play();
        RuntimeManager.PlayOneShot(playerSteps);
    }

    public void Throw()
    {
        RuntimeManager.PlayOneShot(kidThrow);
    }

    public void Pickup()
    {
        RuntimeManager.PlayOneShot(kidPickUp);
    }
}
