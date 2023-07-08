using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using  UnityEngine.Events;

public class Soldier : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public HealthBar healthBar;
    [SerializeField] private Transform movePositionTransform;
    public UnityEvent<Vector3> deathEvent;

    private NavMeshAgent navMeshAgent;
    private void Awake() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject goalObj = GameObject.FindGameObjectWithTag("Goal");
        if (goalObj)
            movePositionTransform = goalObj.transform;

        if(!(navMeshAgent == null) && movePositionTransform != null)
            navMeshAgent.SetDestination(movePositionTransform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            
            deathEvent.Invoke(transform.position);
            Destroy(gameObject);
        }  
    }
}
