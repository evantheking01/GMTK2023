using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopShopUI : MonoBehaviour
{
    public GameObject shopElementPrefab;
    public List<TroopData> shopItems;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in shopItems)
        {
            TroopShopElement shopElement = Instantiate(shopElementPrefab, transform).GetComponent<TroopShopElement>();
            shopElement.Initialize(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
