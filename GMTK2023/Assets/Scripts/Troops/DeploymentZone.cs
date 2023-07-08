using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeploymentZone : MonoBehaviour
{
    [SerializeField] float spawnDelay;
    private float lastSpawnTime;

    private Queue<GameObject> deploymentQueue = new Queue<GameObject>();

    public UnityEvent spawnEvent;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime > spawnDelay)
        {
            if (deploymentQueue.Count > 0)
            {
                GameObject troopPrefab = deploymentQueue.Dequeue();
                Instantiate(troopPrefab, transform);    // relies on prefabs having a zero vector position.
                if (spawnEvent != null)
                    spawnEvent.Invoke();
                lastSpawnTime = Time.time;
            }
        }
    }
    
    public void AddToDeploymentQueue(TroopData troopData)
    {
        for (int i = 0; i < troopData.count; i++)
        {
            deploymentQueue.Enqueue(troopData.troopPrefab);
        }
    }

}
