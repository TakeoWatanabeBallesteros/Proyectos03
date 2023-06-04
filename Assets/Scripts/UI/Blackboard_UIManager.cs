using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackboard_UIManager : DynamicBlackboard
{
    [Header("UI GameObjects")]
    #region MainMenuCanvas
    public GameObject MainMenuCanvas;
    public GameObject SettingsMenuCanvas;
    public GameObject CreditsCanvas;
    public GameObject LevelsMenuCanvas;
    public GameObject LevelInfoCanvas;
    public GameObject CollectableCanvas;
    public GameObject ExitGameCanvas;
    #endregion

    #region InGame
    public GameObject LevelPreviewCanvas;
    public GameObject InGameCanvas;
    public GameObject PauseMenuCanvas;
    public GameObject SettingPauseMenuCanvas;
    public GameObject RestartLevelCanvas;
    public GameObject WinCanvas;
    public GameObject GoMainMenuCanvas;
    #endregion
    
    [Space(5)]
    [Header("Gameplay Objects")] 
    public Slider lifeBar;
    public Slider waterBar;
    public Slider forceBar;
    public TMP_Text TimeLeftText;
    public TMP_Text NumberOfKids;
    public TMP_Text NumberOfColectables;
    public TMP_Text pointsText;
    public TMP_Text pointsWinText;
    public GameObject PickUpText;
    public GameObject ReloadText;
    public GameObject Fire;
    public Image ChildHappyFaceSprite;
    public Image ChildSadFaceSprite;
    public GameObject FireHandle;
    public GameObject PontPopUpOrigin;
    public GameObject PointsPrefab;

    private PlayerControls controls = null;
    private GameManager gameManager;
    private FSM_UIManager uiManager;

    float DeathscreenAlfa;
    public Image YouDiedImage;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Singleton.Instance.GameManager;
        uiManager = Singleton.Instance.UIManager;
        controls = new PlayerControls();
        controls.Enable();
        controls.Player.Pause.performed += ctx =>
        {
            switch (gameManager.gameState)
            {
                case GameState.PauseMenu:
                    uiManager.uiManager_FSM.Trigger("Playing-PauseMenu");
                    break;
                case GameState.Playing: // You trigger between Game & Pause
                    uiManager.uiManager_FSM.Trigger("Playing-PauseMenu");
                    break;
                case GameState.MainMenu:
                    uiManager.WantToExit();
                    break;
                case GameState.SettingsMenu:
                    uiManager.uiManager_FSM.Trigger("MainMenu-SettingsMenu");
                    break;
                case GameState.Credits:
                    uiManager.uiManager_FSM.Trigger("MainMenu-Credits");
                    break;
                case GameState.ExitGame:
                    uiManager.NotSureToExit();
                    break;
                case GameState.LvlsMenu:
                    uiManager.uiManager_FSM.Trigger("MainMenu-LevelsMenu");
                    break;
                case GameState.LvlInfo:
                    LevelInfoCanvas.SetActive(false);
                    break;
                case GameState.LevelPreview:
                    Singleton.Instance.CameraPreviewManager.EndPreview();
                    break;
                case GameState.SettingPause:
                    break;
                case GameState.RestartLvl:
                    break;
            }
        };
        ChildHappyFaceSprite.enabled = false;
        ChildSadFaceSprite.enabled = true;
        FireHandle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLifeBar(float value)
    {
        lifeBar.value = value;
        if (value < 0.5 && !FireHandle.activeSelf)
        {
            FireHandle.SetActive(true);
        }
    }

    public void SetWaterBar(float value)
    {
        waterBar.value = value;
    }

    public void SetTimer(string time)
    {
        TimeLeftText.text = time;
    }

    public void SetKids(int kids, int totalKids)
    {
        NumberOfKids.text = kids+"/"+totalKids;
    }
    public void SetCollectables(int collectables, int totalCollectables)
    {
        NumberOfColectables.text = collectables + "/" + totalCollectables;
    }
    public void SetPoints(int points, int pointsToWin)
    {
        pointsText.text = points + "/" + pointsToWin;
        pointsWinText.text = points + "/" + pointsToWin;
        if(points !=0) PointPopUp(points);
    }

    private void PointPopUp(int points)
    {
        GameObject pref = Instantiate(PointsPrefab, PontPopUpOrigin.transform);
        pref.GetComponent<TextMeshProUGUI>().text = "+" + points;
        pref.transform.position = PontPopUpOrigin.transform.position;
        Destroy(pref, pref.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    public IEnumerator FadeIN()
    {
        DeathscreenAlfa += .1f;
        YouDiedImage.color = new Color(1f, 1f, 1f, DeathscreenAlfa);
        yield return new WaitForSeconds(.1f);
        if (DeathscreenAlfa < 1)
        {
            StartCoroutine(FadeIN());
        }
    }

    public void ChildFace()
    {
        StartCoroutine(ChildSwitchFace());
    }
    private IEnumerator ChildSwitchFace()
    {
        ChildHappyFaceSprite.enabled = true;
        ChildSadFaceSprite.enabled = false;
        yield return new WaitForSeconds(3f);
        ChildHappyFaceSprite.enabled = false;
        ChildSadFaceSprite.enabled = true;
    }
}
