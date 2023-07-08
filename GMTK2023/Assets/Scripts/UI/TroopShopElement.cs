using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class TroopShopElement : MonoBehaviour
{
    public Button purchaseButton;
    public Text countText;

    private TroopData troopData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(TroopData data)
    {
        troopData = data;
        countText.text = troopData.count.ToString();
        purchaseButton.onClick.AddListener(Purchase);
        
    }

    public void Purchase()
    {
        SpawnManager.Instance.SpawnAtActiveZone(troopData);
    }

}
