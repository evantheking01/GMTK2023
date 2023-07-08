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
    [SerializeField] Text moneyText;

    private TroopShopUI shopUI;

    public float CanvasScale { get { return canvasScale; } }
    private float canvasScale;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public void Initialize()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        moneyText.text = "";    // hides this while in main menu

        if (EconomyManager.Instance.moneyChangeEvent == null)
            EconomyManager.Instance.moneyChangeEvent = new UnityEngine.Events.UnityEvent<int>();
        EconomyManager.Instance.moneyChangeEvent.AddListener(UpdateMoneyText);

        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
        // Assuming match will be either 0 or 1. Otherwise this is some more math
        if (canvasScaler.matchWidthOrHeight > 0.5f)
            canvasScale = canvasScaler.referenceResolution.y / Screen.height;
        else
            canvasScale = canvasScaler.referenceResolution.x / Screen.width;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PolulateShop(List<TroopPurchaseData> shopData)
    {
        shopUI = GetComponentInChildren<TroopShopUI>();
        shopUI.Initialize(shopData);
    }

    public void InitializeDeploymentZonesUI(DeploymentZone[] deploymentZones)
    {
        // Delete the existing deployment zones by looking at the children of deploymentZonesPanel
        while (deploymentZonesPanel.transform.childCount > 0)
        {
            DestroyImmediate(deploymentZonesPanel.transform.GetChild(0).gameObject);
        }

        foreach (var zone in deploymentZones)
        {
            DeploymentZoneUIElement zoneUIElement = Instantiate(deploymentZoneUIPrefab, deploymentZonesPanel).GetComponent<DeploymentZoneUIElement>();
            zoneUIElement.SetDeploymentZone(zone);
        }
    }

    private void UpdateMoneyText(int money)
    {
        moneyText.text = money.ToString("C");
    }

    public Transform GetMoneyTextTransform()
    {
        return moneyText.transform;
    }
}
