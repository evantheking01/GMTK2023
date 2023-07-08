using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopShopUI : MonoBehaviour
{
    public GameObject shopGroupPrefab, shopElementPrefab;
    public List<TroopPurchaseData> shopItems;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in shopItems)
        {
            TroopShopGroup shopGroup = Instantiate(shopGroupPrefab, transform).GetComponent< TroopShopGroup>();
            shopGroup.Initialize(item.groupName);
            for (int i = 0; i < item.bulkPurcaseCounts.Length; i++)
            {
                DragAndDropTroopElement shopElement = Instantiate(shopElementPrefab, shopGroup.transform).GetComponent<DragAndDropTroopElement>();
                shopElement.Initialize(item, item.bulkPurcaseCounts[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
