using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TroopPurchaseData : ScriptableObject
{
    public string groupName;
    public GameObject troopPrefab;
    public int costPerUnit;
    public int[] bulkPurcaseCounts;
    
}
