using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public GameObject Player { get => _player; set => _player = null; }
    public GameManager GameManager { get => _gameManager; set => _gameManager = null; }
    public CameraPreviewManager CameraPreviewManager { get => _cameraPreviewManager; set => _cameraPreviewManager = null; }
    public FSM_UIManager UIManager { get => _uiManager; set => _uiManager = null; }
    public PointsBehavior PointsManager { get => _pointsManager; set => _pointsManager = null; }
    public ItemManager ItemsManager { get => _itemsManager; set => _itemsManager = null; }
    public FinalScreenManager FinalScreenManager { get => _finalScreenManager; set => _finalScreenManager = null; }

    #region SerializeFields

    [SerializeField] private GameObject _player;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private CameraPreviewManager _cameraPreviewManager;
    [SerializeField] private FSM_UIManager _uiManager;
    [SerializeField] private PointsBehavior _pointsManager;
    [SerializeField] private ItemManager _itemsManager;
    [SerializeField] private FinalScreenManager _finalScreenManager;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadSingletones;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadSingletones;
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
        _player = GameObject.FindGameObjectWithTag("Player");
        _itemsManager = GameObject.FindGameObjectWithTag("ItemsManager").GetComponent<ItemManager>();
    }

    private void LoadSingletones(Scene scene, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _itemsManager = GameObject.FindGameObjectWithTag("ItemsManager").GetComponent<ItemManager>();
    }
}