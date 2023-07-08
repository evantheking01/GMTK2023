using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

// Class that handles the UI during the core gameplay loop
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance;

    [SerializeField] GameObject deploymentZoneUIPrefab;
    [SerializeField] RectTransform deploymentZonesPanel;

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

    public void InitializeDeploymentZonesUI(DeploymentZone[] deploymentZones)
    {
        // Delete the existing deployment zones by looking at the children of deploymentZonesPanel

        foreach (var zone in deploymentZones)
        {
            DeploymentZoneUIElement zoneUIElement = Instantiate(deploymentZoneUIPrefab, deploymentZonesPanel).GetComponent<DeploymentZoneUIElement>();
            // TODO: implement a class that ties the deployment zone to this UI element and keeps it in the proper screen space on Update tick
            //zoneUIElement.
        }
    }
}
