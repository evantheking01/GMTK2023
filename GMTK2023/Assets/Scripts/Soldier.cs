using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public HealthBar healthBar;
    [SerializeField] private Transform movePositionTransform;

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
            NavMeshPath path = new NavMeshPath();
            if(NavMesh.CalculatePath(transform.position, movePositionTransform.position, navMeshAgent.areaMask, path))
            {
                float totalDistance = Vector3.Distance(transform.position, path.corners[0]);
                for (int i = 1; i < path.corners.Length; i++)
                {
                    totalDistance += Vector3.Distance(path.corners[i], path.corners[i-1]);
                }
                Debug.Log("Soldier died " + totalDistance.ToString() +" feet from the goal!");
            }
            Destroy(gameObject);
        }  
    }
}
