using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TroopData : ScriptableObject
{
    public GameObject troopPrefab;
    public int costPerUnit;
    public int count;
    
}
