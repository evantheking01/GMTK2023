using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalManager : MonoBehaviour
{
    public UnityEvent goalEvent;

    void OnTriggerEnter(Collider collider)
    {   
        if(collider.gameObject.tag == "Soldier")
        {
            Soldier soldier = collider.gameObject.GetComponent<Soldier>();
            soldier.Kill();

            if (goalEvent != null)
            {
                goalEvent.Invoke();
            }
            
        }
        
    }
}
