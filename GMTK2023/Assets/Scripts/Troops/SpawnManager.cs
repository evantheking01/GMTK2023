using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get { return _instance; } }
    private static SpawnManager _instance;

    public DeploymentZone activeZone;

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

    public void SpawnAtActiveZone(TroopData troopData)
    {
        activeZone.AddToDeploymentQueue(troopData);
    }
}
