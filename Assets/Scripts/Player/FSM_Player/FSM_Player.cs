using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Unity.VisualScripting;
using StateMachine = FSM.StateMachine;

public class FSM_Player : MonoBehaviour
{
    #region FSM
    private StateMachine player_FSM;
    #endregion
 
    // Start is called before the first frame update
    private void Start()
    {
        #region FSM
        player_FSM = new StateMachine();
        AddStates();
        AddTransitions();   

        #endregion
    }

    private void AddStates()
    {
        
    }

    private void AddTransitions()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
