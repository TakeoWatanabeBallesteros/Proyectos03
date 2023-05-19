using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FSM_UIManager : MonoBehaviour
{
    #region FSM
    private StateMachine uiManager_FSM;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        #region FSM
        uiManager_FSM = new StateMachine();
        AddStates();
        AddTransitions();   

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region FSM Initialization
    private void AddStates()
    {
        
    }

    private void AddTransitions()
    {
        
    }
    #endregion
}
