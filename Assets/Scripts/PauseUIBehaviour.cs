using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PauseUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void OnEnable()
    {
        // If the game is "PauseMenu" calls EnablePauseUI, else Disable it.
        Singleton.Instance.GameManager.PauseEvent += EnablePauseUI;
        Singleton.Instance.GameManager.UnpauseEvent += DisablePauseUI;
    }

    private void OnDisable()
    {
        Singleton.Instance.GameManager.PauseEvent -= EnablePauseUI;
        Singleton.Instance.GameManager.UnpauseEvent -= DisablePauseUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void EnablePauseUI()
    {
        pauseMenu.SetActive(true);
    }
    
    private void DisablePauseUI()
    {
        pauseMenu.SetActive(false);
    }
}
