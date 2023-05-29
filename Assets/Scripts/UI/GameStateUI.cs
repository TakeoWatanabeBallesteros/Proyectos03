using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gameState_Text;

    private void OnEnable()
    {
        if(Singleton.Instance == null) return;
        Singleton.Instance.GameManager.GameStateChangedEvent += UpdateText;
    }
    
    private void OnDisable()
    {
        Singleton.Instance.GameManager.GameStateChangedEvent -= UpdateText;
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.GameManager.GameStateChangedEvent += UpdateText;
        UpdateText();
    }

    private void UpdateText()
    {
        gameState_Text.text = Singleton.Instance.GameManager.gameState.ToString();
    }
}
