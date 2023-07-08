using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component will persist in all scenes and tracks player progression
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    private int currLevel;

    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject pauseButton;

    void Start()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);

        _instance = this;
        DontDestroyOnLoad(gameObject);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        currLevel = 0; // -1 because LoadnextLevel increments level counter
        LoadNextLevel();

        pauseButton.SetActive(true); // Reveal the pause button once the game starts
        startMenu.SetActive(false);

        // from here leave it to the individual level manager to control the mechanics
    }

    private void LoadNextLevel()
    {
        currLevel++;
        LoadScene.LoadLevel(currLevel);

        // Call level manager to start the first planning phase
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        currLevel = 0;
        LoadScene.LoadLevel(currLevel);

        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);

        startMenu.SetActive(true);
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

}
