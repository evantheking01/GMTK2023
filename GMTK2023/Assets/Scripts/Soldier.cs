using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using  UnityEngine.Events;
public class Soldier : MonoBehaviour
{
    public float maxHealth = 100f;

    public float speed = 1f;

    public float size = 1;
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
        {
            navMeshAgent.SetDestination(movePositionTransform.position);
            navMeshAgent.speed = speed;
        }
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
            
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
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            
            deathEvent.Invoke(transform.position);
            Destroy(gameObject);
        }  
    }

    public void Kill()
    {
        currentHealth = 0;
        healthBar.SetHealth(currentHealth);
        deathEvent.Invoke(transform.position);
        Destroy(gameObject);
    }
}
