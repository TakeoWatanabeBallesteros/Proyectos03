using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatorController : MonoBehaviour
{
    public Animator characterAnimator;

    public void StartAnim()
    {
        characterAnimator.SetTrigger("StartAnim");
    }
    
    public void PickChild()
    {
        characterAnimator.SetTrigger("PickChild");
    }
    public void PrepareChild(bool value)
    {
        if (characterAnimator.GetBool("PrepareChild") != value)
        {
            characterAnimator.SetBool("PrepareChild", value);
        }
    }
    public void YeetChild()
    {
        characterAnimator.SetTrigger("YeetChild");
    }
    public void SetSpeed(int speed)
    {
        characterAnimator.SetFloat("Speed",speed);
    }
    
    public void SetRandomIdle()
    {
        characterAnimator.SetFloat("IdleID", Random.Range(0, 5));
        characterAnimator.SetTrigger("RandomIdle");
    }
}
