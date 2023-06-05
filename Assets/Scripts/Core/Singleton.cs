using System;
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
    public PointsBehavior PointsManager { get => _pointsManager; set => _pointsManager = null; }

#region SerializeFields
[SerializeField] private GameManager _gameManager;
    [SerializeField] private CameraPreviewManager _cameraPreviewManager;
    [SerializeField] private FSM_UIManager _uiManager;
    [SerializeField] private PointsBehavior _pointsManager;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadPlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadPlayer;
    }

    // public AudioManager AudioManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void LoadPlayer(Scene scene, LoadSceneMode mode)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}