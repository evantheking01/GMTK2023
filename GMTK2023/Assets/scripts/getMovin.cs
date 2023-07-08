using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class getMovin : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    private void Awake() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject goalObj = GameObject.FindGameObjectWithTag("Goal");
        if (goalObj)
            movePositionTransform = goalObj.transform;
    }

    private void Update()
    {
        if(!(navMeshAgent == null) && movePositionTransform != null)
            navMeshAgent.destination = movePositionTransform.position;
    }
    
}
