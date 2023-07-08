using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component will persist in all scenes and tracks player progression
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    private int currLevel;


    // Start is called before the first frame update
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
        currLevel = -1; // -1 because LoadnextLevel increments level counter
        LoadNextLevel();

        // from here leave it to the individual level manager to control the mechanics
    }

    private void LoadNextLevel()
    {
        currLevel++;
        LoadScene.LoadLevel(currLevel);

        // Call level manager to start the first planning phase
    }


}
