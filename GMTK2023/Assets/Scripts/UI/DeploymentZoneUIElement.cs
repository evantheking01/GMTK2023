using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class DeploymentZoneUIElement : MonoBehaviour
{

    private Transform deploymentZone;

    [SerializeField] Button deployTroopsButton;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDeploymentZone(DeploymentZone deploymentZone)
    {
        //deployTroopsButton.onClick.AddListener(deploymentZone.DeployTroops)
    }

}
