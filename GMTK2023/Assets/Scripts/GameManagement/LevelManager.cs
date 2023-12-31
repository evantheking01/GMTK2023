using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;


// generically handles a level and transitions between the plan, attack, and review phases
public class LevelManager : MonoBehaviour
{
    [SerializeField] List<TroopPurchaseData> shopData;
    private enum gameState
    {
        planning,
        attack,
        review
    }
    private gameState currentState = gameState.planning;

    private DeploymentZone[] deploymentZones;
    [SerializeField] private int numWaves = 5;
    [SerializeField] private int startingMoney = 100;
    [SerializeField] private GameObject winScreen;

    public int Wave { get { return currWave; } }
    private int currWave;

    private int totalSpawned = 0;
    private int unitCount = 0;
    private int goalCount = 0;

    [SerializeField] private int winCount = 10;
    public int GetUnitCount()
    {
        return unitCount;
    }

    private float minDist = 10000;
    private int moneySpent;
    private bool done;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if(currentState == gameState.attack && unitCount == 0 && EconomyManager.Instance.GetMoney() < 50 && !Input.GetMouseButton(0))
        {
            currentState = gameState.planning;
            EndWave();
        }
        else if(currentState == gameState.attack && unitCount == 1 && EconomyManager.Instance.GetMoney() < 50 && !Input.GetMouseButton(0))
        {
            Soldier soldier = GameObject.FindFirstObjectByType<Soldier>();
            soldier.playScreamOnDeath = true;
        }
    }

    public void Initialize()
    {
        // attaching events for spawning units
        deploymentZones = GameObject.FindObjectsOfType<DeploymentZone>();
        UIManager.Instance.InitializeDeploymentZonesUI(deploymentZones);
        for (int i = 0; i < deploymentZones.Length; i++)
        {
            deploymentZones[i].spawnEvent = new UnityEvent<Soldier>();
            deploymentZones[i].spawnEvent.AddListener(incrementUnitCount);
        }
        
        UIManager.Instance.PolulateShop(shopData);

        GoalManager goalManager = GameObject.FindObjectOfType<GoalManager>();
        goalManager.goalEvent.AddListener(GoalReached);

        currWave = 0;
        StartWave();
        UIManager.Instance.UpdateGoalProgress($"{goalCount}/{winCount}");
        EconomyManager.Instance.SetMoney(startingMoney);
    }

    public void GoalReached()
    {
        goalCount++;
        UIManager.Instance.UpdateGoalProgress($"{goalCount}/{winCount}");
        if (goalCount >= winCount)
        {
            EndWave();
        }
    }

    private void incrementUnitCount(Soldier soldier)
    {
        // if we were in the planning state, update to attack state.
        if(currentState == gameState.planning)
            currentState = gameState.attack;

        // attach event to wait for their death
        if(soldier.deathEvent == null)
            soldier.deathEvent = new UnityEvent<Vector3>();
        soldier.deathEvent.AddListener(decrementUnitCount);

        unitCount++;
        totalSpawned++;
        Debug.Log("Unit count = " + unitCount.ToString());
    }

    private void decrementUnitCount(Vector3 position)
    {
        // compute the distance the unit traveled from spawn
        NavMeshPath path = new NavMeshPath();

        // compute the distance form their death position to the goal at time of death
        Vector3 goalPosition = new Vector3(0, 0, 0);
        GameObject goalObj = GameObject.FindGameObjectWithTag("Goal");
        if (goalObj)
            goalPosition = goalObj.transform.position;
        float distanceToGoal = 0f;
        if (NavMesh.CalculatePath(goalPosition, position, NavMesh.AllAreas, path))
        {
            distanceToGoal = Vector3.Distance(goalPosition, path.corners[0]);
            for (int i = 1; i < path.corners.Length; i++)
            {
                distanceToGoal += Vector3.Distance(path.corners[i], path.corners[i - 1]);
            }
        }

        if (distanceToGoal < minDist)
        {
            minDist = distanceToGoal;
            // TODO: place a flag marker here
        }
        unitCount--;
        Debug.Log("Unit count = " + unitCount.ToString());
    }

    public void EndWave()
    {
        if (done) return;   // so we dont show ui again when money is spent after all guys make it
        done = true;

        Soldier[] allSoldiers = FindObjectsOfType<Soldier>();
        for (int i = 0; i < allSoldiers.Length; i++)
        {
            if (allSoldiers[i] != null)
                allSoldiers[i].Kill();
        }

        EconomyManager.Instance.attackPhaseOver();
        GameManager.Instance.WaveComplete(totalSpawned);
        // we have lost
        if (currWave >= numWaves)
        {
            GameManager.Instance.LevelEnd(false);
            return;
        }

        // we have won
        if (goalCount >= winCount)
        {
            GameManager.Instance.LevelEnd(true);
            return;
        }
        
        // round over. vibin'
        UIManager.Instance.ShowEndWaveUI(
            totalSpawned,
            moneySpent,
            EconomyManager.Instance.AttackMoney,
            EconomyManager.Instance.GetMoney(),
            currWave
            );

    }

    public void StartWave()
    {
        done = false;
        currWave++;
        totalSpawned = 0;
        moneySpent = 0;
        UIManager.Instance.SetWaveText(currWave, numWaves);
        UIManager.Instance.UpdatePotentialEarnings(0);
        EconomyManager.Instance.StartPlanningPhase();
        // ui manager to update wave
    }
}
