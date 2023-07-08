using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = .1f;

    public int damage = 50;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
		{
			Destroy(gameObject);
			return;
		}

        Vector3 dir = target.position - transform.position;
        float distanceToTravel = speed * Time.deltaTime;

        if(dir.magnitude <= distanceToTravel)
        {
            HitTarget();
            return;
        }

        transform.Translate( dir.normalized * distanceToTravel, Space.World);
        transform.LookAt(target);
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void HitTarget()
    {
        Debug.Log("HIT!");
        Destroy(gameObject);
    }
}
