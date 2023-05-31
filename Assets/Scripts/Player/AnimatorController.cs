using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator CharacterAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickChild()
    {
        CharacterAnimator.SetTrigger("PickChild");
    }
    public void PrepareChild()
    {
        CharacterAnimator.SetTrigger("PrepareChild");
    }
    public void YeetChild()
    {
        CharacterAnimator.SetTrigger("YeetChild");
    }
    public void SetSpeed(int speed)
    {
        CharacterAnimator.SetFloat("Speed",speed);
    }
}
