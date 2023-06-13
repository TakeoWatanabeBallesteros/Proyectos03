using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem walkingParticles;
    [SerializeField] private EventReference playerSteps;
    

    public void OnStep()
    {
        walkingParticles.Play();
        RuntimeManager.PlayOneShot(playerSteps);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
