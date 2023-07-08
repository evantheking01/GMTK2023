using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopShopUI : MonoBehaviour
{
    public GameObject shopGroupPrefab, shopElementPrefab;
    public List<TroopPurchaseData> shopItems;

    private List<GameObject> shopGroupElements;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(List<TroopPurchaseData> shopData)
    {
        shopItems = shopData;

        if (shopGroupElements != null)
        {
            for (int i = 0; i < shopGroupElements.Count; i++)
            {
                Destroy(shopGroupElements[i]);
            }
        }
        
        shopGroupElements = new List<GameObject>();
        
        foreach (var item in shopItems)
        {
            TroopShopGroup shopGroup = Instantiate(shopGroupPrefab, transform).GetComponent< TroopShopGroup>();
            shopGroup.Initialize(item.groupName);
            shopGroupElements.Add(shopGroup.gameObject);
            for (int i = 0; i < item.bulkPurcaseCounts.Length; i++)
            {
                DragAndDropTroopElement shopElement = Instantiate(shopElementPrefab, shopGroup.transform).GetComponent<DragAndDropTroopElement>();
                shopElement.Initialize(item, item.bulkPurcaseCounts[i]);
                shopElement.name = shopElement.name.Replace("(Clone)", $" {item.bulkPurcaseCounts[i]} {item.groupName}s");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetFirstShopItem()
    {
        if (shopGroupElements == null) return null;
        if (shopGroupElements.Count == 0) return null;

        return shopGroupElements[0].transform.GetChild(1);
    }

}
