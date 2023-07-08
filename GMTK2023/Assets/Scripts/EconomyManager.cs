using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get { return _instance; } }
    private static EconomyManager _instance;

    public UnityEvent<int> moneyChangeEvent;
    public UnityEvent soldierDiedEvent;
    public float deathMinimumMoney = 0f;
    public float deathMaximumMoney = 1000f;

    private int money;
    private int attackMoney;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void BroadcastMoneyCount()
    {
        if (moneyChangeEvent != null) 
        {
            moneyChangeEvent.Invoke(money);
        }
    }

    public void IncreaseMoney(int amount)
    {
        money += amount;
        BroadcastMoneyCount();
    }

    public void DecreaseMoney(int amount)
    {
        money -= amount;
        BroadcastMoneyCount();
    }

    public void SetMoney(int amount)
    {
        money = amount;
        BroadcastMoneyCount();
    }

    public bool CanAfford(int amount)
    {
        return amount <= money;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SoldierDied(float pathCompletionRatio)
    {
        float moneyAwarded = ((deathMaximumMoney - deathMinimumMoney) * pathCompletionRatio) + deathMinimumMoney;
        attackMoney += (int) moneyAwarded;
    }

    public void attackPhaseOver()
    {
        IncreaseMoney(attackMoney);
        attackMoney = 0;
    }

}
