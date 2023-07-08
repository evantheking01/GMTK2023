using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class DeploymentZoneUIElement : MonoBehaviour
{
    private Vector2 offset = new Vector2(-40, -40);

    private RectTransform rectTransform;
    public DeploymentZone ZoneOfDeployment { get { return deploymentZone; } }
    private DeploymentZone deploymentZone;

    [SerializeField] Button deployTroopsButton;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deploymentZone)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(deploymentZone.transform.position);
            transform.position = screenPos + offset;
        }
    }

    public void SetDeploymentZone(DeploymentZone dz)
    {
        //deployTroopsButton.onClick.AddListener(deploymentZone.DeployTroops)
        deploymentZone = dz;
    }

}
