using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public float maxHealth = 100f;

    public float speed = 1f;

    public float size = 1;
    private float currentHealth;
    public HealthBar healthBar;

    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);

        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject goalObj = GameObject.FindGameObjectWithTag("Goal");
        if (goalObj)
            movePositionTransform = goalObj.transform;

        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!(navMeshAgent == null) && movePositionTransform != null)
            navMeshAgent.destination = movePositionTransform.position;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }  
    }
}
