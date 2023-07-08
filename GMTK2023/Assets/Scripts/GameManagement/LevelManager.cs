using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// generically handles a level and transitions between the plan, attack, and review phases
public class LevelManager : MonoBehaviour
{
    [SerializeField] List<TroopPurchaseData> shopData;

    private DeploymentZone[] deploymentZones;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void Initialize()
    {
        deploymentZones = GameObject.FindObjectsOfType<DeploymentZone>();
        UIManager.Instance.InitializeDeploymentZonesUI(deploymentZones);
        UIManager.Instance.PolulateShop(shopData);
    }
}
