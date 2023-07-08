using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string targetTag;

    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject bulletPrefab;

    private Transform target;

    [SerializeField] private float fireRate = 1f;

    float fireCountdown = 0f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float shorestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach(GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < shorestDistance)
            {
                shorestDistance = distance;
                nearestTarget = enemy;
            }
        }

        if(nearestTarget != null && shorestDistance <= radius)
        {
            target = nearestTarget.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if(target != null)
        {
            // Vector3 direction = target.position - transform.position;
            // Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Vector3 rotation = Quaternion.Lerp(gun.rotation, lookRotation, Time.deltaTime).eulerAngles;
            // gun.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            gun.LookAt(target);

            if(fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }

    }

    

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Bullet bullet = bulletGO.GetComponent<Bullet>();

                if (bullet != null)
                    bullet.Seek(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
