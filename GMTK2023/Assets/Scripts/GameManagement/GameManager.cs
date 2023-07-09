using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This component will persist in all scenes and tracks player progression
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    [SerializeField] UIManager uiManager;
    [SerializeField] EconomyManager economyManager;

    private int currLevel;

    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject pauseButton;

    public GameObject winScreen;

    [SerializeField] int startingMoney = 100;
    [SerializeField] GameObject sceneChangeSound;

    private int totalUnitsSpawned;
    private int startingLevelMoney;
    private int totalMoneySpent;

    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        
        economyManager.Initialize();
        uiManager.Initialize();
        economyManager.SetMoney(startingMoney);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name != "Start Menu") // for testing
        {
            Debug.Log("starting a test level");
            StartLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if (pauseButton)
        {
            pauseButton.SetActive(true); // Reveal the pause button once the game starts
        }
        if (startMenu)
        {
            startMenu.SetActive(false);
        }

        currLevel = 0;
        LoadNextLevel();
    }

    private void StartLevel()
    {
        // Call level manager to start the first planning phase
        LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
        if (levelManager)
        {
            levelManager.Initialize();
        }
    }

    public void LevelEnd(bool win)
    {
        LevelManager lm = FindObjectOfType<LevelManager>();
        UIManager.Instance.ShowEndWaveUI(
            totalUnitsSpawned,
            totalMoneySpent,
            EconomyManager.Instance.GetMoney() - startingLevelMoney,
            EconomyManager.Instance.GetMoney(),
            lm.Wave,
            win
            );

        //If this is the last level, show the win screen
        if(SceneManager.sceneCount-1 == SceneManager.GetActiveScene().buildIndex)
        {
            if(winScreen != null)
            {
                winScreen.SetActive(true);
            }
        }
        

        totalUnitsSpawned = 0;
        totalMoneySpent = 0;
        startingLevelMoney = EconomyManager.Instance.GetMoney();
    }

    public void LoadNextLevel()
    {
        currLevel++;
        totalUnitsSpawned = 0;
        LoadScene.LoadLevel(currLevel);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject sceneChange = Instantiate(sceneChangeSound, transform.position, transform.rotation);
        Destroy(sceneChange, .2f);
        StartLevel();
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        currLevel = 0;
        LoadScene.LoadLevel(currLevel);

        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);

        startMenu.SetActive(true);

        startingLevelMoney = EconomyManager.Instance.GetMoney();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        if(pauseMenu)
        {
            pauseMenu.SetActive(true);
            pauseButton.SetActive(false);
        }

    }

    public void ResumeGame()
    {
        if(pauseMenu)
        {
            pauseMenu.SetActive(false);
            pauseButton.SetActive(true);
        }
        Time.timeScale = 1.0f;
    }

    public void WaveComplete(int troopsDeployed)
    {
        totalUnitsSpawned += troopsDeployed;
    }

    public void UpdateLifetimeEconomy(int moneySpent)
    {
        totalMoneySpent += moneySpent;
    }

}
