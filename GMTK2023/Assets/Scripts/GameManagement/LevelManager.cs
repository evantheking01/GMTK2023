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
    private int unitCount = 0;
    private int goalCount = 0;

    private int winCount = 10;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if(currentState == gameState.attack && unitCount == 0 && EconomyManager.Instance.GetMoney() < 50 && !Input.GetMouseButton(0))
        {
            // todo: change this state transition to go to review once we know wtf that means
            currentState = gameState.planning;

            //todo: increment wave number

            EconomyManager.Instance.attackPhaseOver();
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
    }

    public void GoalReached()
    {
        goalCount++;

        if(goalCount > winCount)
        {
            Debug.Log("LEVEL WON");
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
    }

    private void decrementUnitCount(Vector3 position)
    {
        unitCount--;
    }
}
