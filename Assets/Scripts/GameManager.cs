using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int SavedKids = 0;
    [SerializeField] private GameObject[] Kids;
    [SerializeField] private int TotalKids = 0;

    [SerializeField] private int Collected = 0;
    [SerializeField] private GameObject[] Collectables;
    [SerializeField] private int TotalCollectables = 0;

    PlayerHealth PH;
    public TMP_Text NumberOfkids;
    public TMP_Text NumberOfCollectables;
    public TMP_Text TimeLeftText;
    [SerializeField] private float TimerEnSegundos;
    int minutes, seconds, cents;
    public GameObject winScreen;
    
    // That maybe should not be here
    private PlayerControls controls = null;
    
    
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        //controls.Player.Pause
    }

    // Start is called before the first frame update
    void Start()
    {
        PH = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        DontDestroyOnLoad(this);

        Kids = GameObject.FindGameObjectsWithTag("Kid");
        TotalKids = Kids.Length;

        Collectables = GameObject.FindGameObjectsWithTag("Collectable");
        TotalCollectables = Collectables.Length;

        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerEnSegundos > 0)
            TimerEnSegundos -= Time.deltaTime;
        if ( TimerEnSegundos < 0)
            TimerEnSegundos = 0;
        if (TimerEnSegundos == 0)
            PH.IntantDeath();
        minutes = (int)(TimerEnSegundos / 60f);
        seconds = (int)(TimerEnSegundos - minutes * 60f);
        cents = (int)((TimerEnSegundos - (int)TimerEnSegundos) * 100f);

        TimeLeftText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
        NumberOfkids.text = ("Kids saved: " + SavedKids + "/" + TotalKids);
        NumberOfCollectables.text = ("Collectables: " + Collected + "/" + TotalCollectables);

        if(TimerEnSegundos>0 && SavedKids >= TotalKids)
        {
            Debug.Log("Conseguiste salvar a los niños");
            Win();
        }
    }
    public int GetTotalKids()
    {
        return TotalKids;
    }
    public int GetSavedKids()
    {
        return TotalKids;
    }
    public void AddChild()
    {
        SavedKids++;
    }
    public void AddCollectable()
    {
        Collected++;
    }
    public void AddTime(float Time)
    {
        TimerEnSegundos += Time;
    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
