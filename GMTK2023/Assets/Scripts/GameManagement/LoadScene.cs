using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance { get { return _instance; } }
    private static LoadScene _instance;

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

    public static void LoadLevel(int levelIndex)
    {
        // "Level1" Level + levelIndex.ToString()
        if (SceneManager.sceneCountInBuildSettings <= levelIndex)
            levelIndex = SceneManager.sceneCountInBuildSettings - 1;
        Debug.Log("Load scene at index " + levelIndex);
        SceneManager.LoadScene(levelIndex);
    }
}
