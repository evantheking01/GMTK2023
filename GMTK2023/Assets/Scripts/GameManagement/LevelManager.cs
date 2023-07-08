using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// generically handles a level and transitions between the plan, attack, and review phases
public class LevelManager : MonoBehaviour
{
    private DeploymentZone[] deploymentZones;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // TODO: Get all the possible spawn points by the class name
        deploymentZones = GameObject.FindObjectsOfType<DeploymentZone>();
        UIManager.Instance.InitializeDeploymentZonesUI(deploymentZones);

        // 
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
