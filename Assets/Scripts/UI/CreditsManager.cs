using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{

    Animator anim;
    //InputPlayerController Input;
    private FSM_UIManager uiManager;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        //Input = GetComponent<InputPlayerController>();
        uiManager = Singleton.Instance.UIManager;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReturnToMenu();
        AccelerateCreddits();
    }
    private void AccelerateCreddits()
    {

        if (Input.GetKey(KeyCode.Space)) anim.speed = 3;
        else anim.speed = 1;
         
    }

    //When credits end, you are bringed back to the menu
    private void ReturnToMenu()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) return;
        uiManager.MainMenuCredits();
    }
}
