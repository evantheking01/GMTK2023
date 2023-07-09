using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private RectTransform hintObject, hintRectText;
    private Text hintText;
    
    private DragAndDropTroopElement shopElement;
    private DeploymentZoneUIElement dzElement;

    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        hintText = hintObject.GetComponentInChildren<Text>();
        Invoke("TeachPurchase", 0.5f);
    }

    private void TeachPurchase()
    {
        TroopShopUI troopShop = GameObject.FindObjectOfType<TroopShopUI>();
        if (troopShop == null)
            return;
        
        Transform smallestUnit = troopShop.GetFirstShopItem();
        
        shopElement = smallestUnit.GetComponent<DragAndDropTroopElement>();
        //hintObject.position = shopElement.transform.position + new Vector3(0, 40, 0);
        hintObject.SetParent(smallestUnit);
        hintObject.anchoredPosition = new Vector2();
        hintObject.SetParent(transform);
        hintText.text = "Click on a troop in the shop";

        if (shopElement.grabEvent == null)
            shopElement.grabEvent = new UnityEvent();
        shopElement.grabEvent.RemoveListener(TeachSpawn);
        shopElement.grabEvent.AddListener(TeachSpawn);

        if (shopElement.dropEvent == null)
            shopElement.dropEvent = new UnityEvent<bool>();
        shopElement.dropEvent.RemoveListener(UnitsSpawned);
        shopElement.dropEvent.AddListener(UnitsSpawned);
    }

    private void TeachSpawn()
    {
        dzElement = GameObject.FindObjectOfType<DeploymentZoneUIElement>();

        hintObject.SetParent(transform);
        hintObject.position = dzElement.transform.position + new Vector3(0, -30, 0);
        hintObject.SetParent(transform);

        hintText.text = "Drag troops here";
    }

    private void UnitsSpawned(bool success)
    {
        shopElement.grabEvent.RemoveListener(TeachSpawn);
        shopElement.dropEvent.RemoveListener(UnitsSpawned);

        if (success)
        {
            TeachMoney();
        }
        else
        {
            TeachPurchase();
        }
    }

    private void TeachMoney()
    {
        done = true;
        //hintObject.anchoredPosition = UIManager.Instance.GetMoneyTextTransform() + new Vector3(0, 40, 0);
        Transform moneyTransform = UIManager.Instance.GetMoneyTextTransform();

        hintObject.SetParent(moneyTransform);
        hintObject.anchoredPosition = new Vector2(50, 0);
        hintObject.SetParent(transform);
        hintObject.sizeDelta += new Vector2(0, 0);
        hintRectText.sizeDelta += new Vector2(100, 0);
        hintText.text = "Purchasing troops costs money. Wave complete when you are poor.";
    }

    private void Update()
    {
        if (done == true && hintObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(hintObject.gameObject);
            }
        }
    }

}
