using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;
using Random = UnityEngine.Random;

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
    public GameObject HowToPlayCanvas;
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
    public TMP_Text SombraOfKids;
    public TMP_Text NumberOfColectables;
    public TMP_Text SombraOfColectables;
    public TMP_Text pointsText;
    public TMP_Text SombraOfPoints;
    public TMP_Text pointsWinText;
    public GameObject PickUpText;
    public GameObject ReloadText;
    public GameObject Fire;
    public Image ChildHappyFaceSprite;
    public Image ChildSadFaceSprite;
    public GameObject FireHandle;
    public GameObject PontPopUpOrigin;
    public GameObject PointsPrefab;
    public GameObject RedPointsPrefab;
    public GameObject GreenPointsPrefab;

    private PlayerControls controls = null;
    private GameManager gameManager;
    private FSM_UIManager uiManager;

    float DeathscreenAlfa;
    public Image YouDiedImage;
    public Image TimesUpImage;

    public List<WinningImage> winningImages;

    private VCA SFX;
    private VCA Music;
    private VCA Ambient;

    private float SFXVoulme;
    private float MusicVoulme;
    private float AmbientVoulme;

    public Slider SFX_0;
    public Slider SFX_1;
    public Slider Music_0;
    public Slider Music_1;
    public Slider Ambient_0;
    public Slider Ambient_1;

    private void Awake()
    {
        SFX = RuntimeManager.GetVCA("vca:/SFX");
        Music = RuntimeManager.GetVCA("vca:/Music");
        Ambient = RuntimeManager.GetVCA("vca:/Ambient");
    }

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
                case GameState.HowToPlay:
                    uiManager.uiManager_FSM.Trigger("MainMenu-HowToPlay");
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

    public void SetLifeBar(float value)
    {
        lifeBar.value = value;
        if (value <= 50 && !FireHandle.activeSelf)
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
        SombraOfKids.text = kids+"/"+totalKids;
        
    }
    public void SetCollectables(int collectables, int totalCollectables)
    {
        NumberOfColectables.text = collectables + "/" + totalCollectables;
        SombraOfColectables.text = collectables + "/" + totalCollectables;
    }
    public void SetPoints(int points, int pointsToWin, int currentPoints)
    {
        pointsText.text = currentPoints + "/" + pointsToWin;
        SombraOfPoints.text = currentPoints + "/" + pointsToWin;
        pointsWinText.text = currentPoints + "/" + pointsToWin;
        if (points == 0) return;
        if (points < 0) RedPointsPopUp(points);
        else PointsPopUp(points);

    }


    private void PointsPopUp(int points)
    {
        GameObject pref = Instantiate(PointsPrefab, PontPopUpOrigin.transform); 
        pref.GetComponent<TextMeshProUGUI>().text = points < 0 ?  "" + points : "+" + points;
        Vector3 a = new Vector3(PontPopUpOrigin.transform.position.x, PontPopUpOrigin.transform.position.y, PontPopUpOrigin.transform.position.z);
        a.x += Random.Range(-40,40);
        a.y += Random.Range(-40, 40);
        pref.transform.position = a;
        Destroy(pref, pref.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.1f);

    }
    private void RedPointsPopUp(int points)
    {
        GameObject pref = Instantiate(RedPointsPrefab, PontPopUpOrigin.transform);
        pref.GetComponent<TextMeshProUGUI>().text = points < 0 ? "" + points : "+" + points;
        Vector3 a = new Vector3(PontPopUpOrigin.transform.position.x, PontPopUpOrigin.transform.position.y, PontPopUpOrigin.transform.position.z);
        a.x += Random.Range(-40, 40);
        a.y += Random.Range(-40, 40);
        pref.transform.position = a;
        Destroy(pref, pref.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.1f);

    }
    private void GreenPointsPopUp(int points)
    {
        GameObject pref = Instantiate(GreenPointsPrefab, PontPopUpOrigin.transform);
        pref.GetComponent<TextMeshProUGUI>().text = points < 0 ? "" + points : "+" + points;
        Vector3 a = new Vector3(PontPopUpOrigin.transform.position.x, PontPopUpOrigin.transform.position.y, PontPopUpOrigin.transform.position.z);
        a.x += Random.Range(-40, 40);
        a.y += Random.Range(-40, 40);
        pref.transform.position = a;
        Destroy(pref, pref.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.1f);

    }

    public IEnumerator FadeIN(bool timesup)
    {
        yield return new WaitForSeconds(2f);
        DeathscreenAlfa = 0;
        for (float i=0; i <1; i+=.1f)
        {
            DeathscreenAlfa += .1f;
            //Takeo perdoname
            if (timesup)
            {
                TimesUpImage.color = new Color(1f, 1f, 1f, DeathscreenAlfa);
            }
            else
            {

                YouDiedImage.color = new Color(1f, 1f, 1f, DeathscreenAlfa);
            }
            yield return new WaitForSeconds(.05f);
        }
    }
    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);

        for (float i = 0; i < 1; i += .1f)
        {
            DeathscreenAlfa -= .2f;
            YouDiedImage.color = new Color(1f, 1f, 1f, DeathscreenAlfa);
            yield return new WaitForSeconds(.05f);
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

    public void OnSFXChange(float value) {
        SFX.setVolume(value);
        SFX_0.value = value;
        SFX_1.value = value;
    }
    public void OnMusicChange(float value) {
        Music.setVolume(value);
        Music_0.value = value;
        Music_1.value = value;
    }
    public void OnAmbientChange(float value) {
        Ambient.setVolume(value);
        Ambient_0.value = value;
        Ambient_1.value = value;
    }
}
