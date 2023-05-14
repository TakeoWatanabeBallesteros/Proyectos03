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
        Singleton.Instance.GameManager.PauseEvent += UpdateText;;
        Singleton.Instance.GameManager.UnpauseEvent += UpdateText;;
    }
    
    private void OnDisable()
    {
        Singleton.Instance.GameManager.PauseEvent -= UpdateText;
        Singleton.Instance.GameManager.UnpauseEvent -= UpdateText;;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState_Text.text = Singleton.Instance.GameManager.gameState.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateText()
    {
        gameState_Text.text = Singleton.Instance.GameManager.gameState.ToString();
    }
}
