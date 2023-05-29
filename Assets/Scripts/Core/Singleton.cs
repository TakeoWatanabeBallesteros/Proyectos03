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
    public FSM_UIManager UIManager { get => _uiManager; set => _uiManager = null; }
    

    #region SerializeFields
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CameraPreviewManager _cameraPreviewManager;
    [SerializeField] private FSM_UIManager _uiManager;
    #endregion

    // public AudioManager AudioManager { get; private set; }
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
    }
}