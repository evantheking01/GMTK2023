using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeploymentZone : MonoBehaviour
{
    public UnityEvent spawnEvent;

    [SerializeField] float spawnDelay = 0.05f;
    private float lastSpawnTime;

    private List<GameObject> troopDatabase = new List<GameObject>();
    private Queue<int> deploymentQueue = new Queue<int>();



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
                GameObject troopPrefab = troopDatabase[deploymentQueue.Dequeue()];
                Instantiate(troopPrefab, transform);    // relies on prefabs having a zero vector position.
                if (spawnEvent != null)
                    spawnEvent.Invoke();
                lastSpawnTime = Time.time;
            }
        }
    }

    public void AddToDeploymentQueue(TroopPurchaseData troopData, int troopCount)
    {
        int troopID = -1;
        for (int i = 0; i < troopDatabase.Count; i++)
        {
            if (troopDatabase[i] == troopData.troopPrefab)
            {
                Debug.Log("prefabs same");
                troopID = i;
            }
        }

        // register new prefab
        if (troopID < 0)
        {
            troopID = troopDatabase.Count;
            troopDatabase.Add(troopData.troopPrefab);
        }

        for (int i = 0; i < troopCount; i++)
            deploymentQueue.Enqueue(troopID);
    }

}
