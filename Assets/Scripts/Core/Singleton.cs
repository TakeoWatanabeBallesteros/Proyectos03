using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public GameManager GameManager { get; private set; }

    // public AudioManager AudioManager { get; private set; }
    // public UIManager UIManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        GameManager = GetComponentInChildren<GameManager>();
        // AudioManager = GetComponentInChildren<AudioManager>();
        // UIManager = GetComponentInChildren<UIManager>();
    }
}