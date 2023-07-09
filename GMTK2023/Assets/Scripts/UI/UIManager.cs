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
    [SerializeField] WorldSpaceUIElement goalProgressElement;
    [SerializeField] Text waveText;
    [SerializeField] NumberCounter moneyText, potentialEarningsText;
    [SerializeField] Button retreatButton;
    private CanvasScaler canvasScaler;
    
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

        moneyText.QuickText("");    // hides this while in main menu

        if (EconomyManager.Instance.moneyChangeEvent == null)
            EconomyManager.Instance.moneyChangeEvent = new UnityEngine.Events.UnityEvent<int>();
        EconomyManager.Instance.moneyChangeEvent.AddListener(UpdateMoneyText);

        canvasScaler = GetComponentInParent<CanvasScaler>();
        // Assuming match will be either 0 or 1. Otherwise this is some more math
        if (canvasScaler.matchWidthOrHeight > 0.5f)
            canvasScale = canvasScaler.referenceResolution.y / Screen.height;
        else
            canvasScale = canvasScaler.referenceResolution.x / Screen.width;
        StartCoroutine(RefreshCanvasScale());

        retreatButton.onClick.AddListener(endWave);
        retreatButton.gameObject.SetActive(false);
    }

    private void endWave()
    {
        LevelManager thisLevel = GameObject.FindObjectOfType<LevelManager>();
        if (thisLevel != null)
        {
            thisLevel.EndWave();
        }
    }

    private IEnumerator RefreshCanvasScale()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // Assuming match will be either 0 or 1. Otherwise this is some more math
            if (canvasScaler.matchWidthOrHeight > 0.5f)
                canvasScale = canvasScaler.referenceResolution.y / Screen.height;
            else
                canvasScale = canvasScaler.referenceResolution.x / Screen.width;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PolulateShop(List<TroopPurchaseData> shopData)
    {
        shopUI = GetComponentInChildren<TroopShopUI>();
        shopUI.Initialize(shopData);
        GoalManager goal = GameObject.FindObjectOfType<GoalManager>();
        goalProgressElement.target = goal.transform;

        retreatButton.gameObject.SetActive(true);
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
        moneyText.useLastValue = true ;
        moneyText.SetText(0, money, true, 0.5f, "C");
        //moneyText.text = money.ToString("C");
    }

    public void UpdatePotentialEarnings(int value)
    {
        potentialEarningsText.useLastValue = true;
        potentialEarningsText.SetText(0, value, true, 0.5f, "C", "", "+");
        //potentialEarningsText.text = $"+{value.ToString("C")}";
    }

    public Transform GetMoneyTextTransform()
    {
        return moneyText.transform;
    }

    public void SetWaveText(int currWave, int numWaves)
    {
        waveText.text = $"Wave: {currWave}/{numWaves}";
        shopUI.EnableShop();
    }

    public void ShowEndWaveUI(int numTroops, int moneySpent, int moneyEarned, int balance, int wave, bool levelComplete=false)
    {
        WaveEndScreen endScreen = GetComponentInChildren<WaveEndScreen>();
        endScreen.SetWaveText(wave, levelComplete);
        endScreen.Populate(numTroops, moneySpent, moneyEarned, balance);
        shopUI.DisableShop();
    }

    public void UpdateGoalProgress(string str)
    {
        goalProgressElement.SetText(str);
    }
}
