using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class DeploymentZone : MonoBehaviour
{
    public UnityEvent<Soldier> spawnEvent;

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
                Soldier soldier = Instantiate(troopPrefab, transform).GetComponent<Soldier>();    // relies on prefabs having a zero vector position.
                soldier.deathEvent = new UnityEvent<Vector3>();
                soldier.deathEvent.AddListener(calculateDeathDistance);

                if (spawnEvent != null)
                    spawnEvent.Invoke(soldier);
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

    private void calculateDeathDistance(Vector3 deathPostion)
    {
        // compute the distance the unit traveled from spawn
        NavMeshPath path = new NavMeshPath();
        float distanceTraveled = 0f;
        if(NavMesh.CalculatePath(transform.position, deathPostion, NavMesh.AllAreas, path))
        {
            distanceTraveled = Vector3.Distance(transform.position, path.corners[0]);
            for (int i = 1; i < path.corners.Length; i++)
            {
                distanceTraveled += Vector3.Distance(path.corners[i], path.corners[i-1]);
            }
        }

        // compute the distance form their death position to the goal at time of death
        Vector3 goalPosition = new Vector3(0,0,0);
        GameObject goalObj = GameObject.FindGameObjectWithTag("Goal");
        if (goalObj)
            goalPosition = goalObj.transform.position;
        float distanceToGoal = 0f;
        if(NavMesh.CalculatePath(goalPosition, deathPostion, NavMesh.AllAreas, path))
        {
            distanceToGoal = Vector3.Distance(goalPosition, path.corners[0]);
            for (int i = 1; i < path.corners.Length; i++)
            {
                distanceToGoal += Vector3.Distance(path.corners[i], path.corners[i-1]);
            }
        }

        // ratio that shit
        float pathCompletionRatio = distanceTraveled / (distanceTraveled + distanceToGoal);
        Debug.Log("test");
        EconomyManager.Instance.SoldierDied(pathCompletionRatio);

    }

}
