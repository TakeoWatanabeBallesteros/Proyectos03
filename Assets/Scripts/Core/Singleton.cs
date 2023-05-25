using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public GameObject Player { get; private set; }
    public GameManager GameManager { get => _gameManager; set => _gameManager = null; }
    public CameraPreviewManager CameraPreviewManager { get => _cameraPreviewManager; set => _cameraPreviewManager = null; }

    #region SerializeFields
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CameraPreviewManager _cameraPreviewManager;
    #endregion

    // public AudioManager AudioManager { get; private set; }
    // public UIManager UIManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Player = GameObject.FindGameObjectWithTag("Player");
        // AudioManager = GetComponentInChildren<AudioManager>();
        // UIManager = GetComponentInChildren<UIManager>();
    }
    
    
}