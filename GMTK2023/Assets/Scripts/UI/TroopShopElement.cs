using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

// Simple clickable button for spawning units
// THIS IS MOSTLY FOR TESTING
public class TroopShopElement : MonoBehaviour
{
    public Button purchaseButton;
    public Text countText;

    private TroopPurchaseData troopData;
    private int troopCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(TroopPurchaseData data, int count)
    {
        troopData = data;
        troopCount = count;
        countText.text = troopCount.ToString();
        purchaseButton.onClick.AddListener(Purchase);
        
    }

    public void Purchase()
    {
        SpawnManager.Instance.SpawnAtActiveZone(troopData, troopCount);
    }

}
