using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
public class Turret : MonoBehaviour
{
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string targetTag;

    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject bulletPrefab;

    private Transform target;

    [SerializeField] private float fireRate = 1f;

    [SerializeField] private int damage = 25;

    float fireCountdown = 0f;

    enum Priority {Speed, Size, Close};

    [SerializeField] private Priority priority;

    [SerializeField] private GameObject helpCanvas;

    [SerializeField] private Text helpText;

    [SerializeField] private AudioClip gunshot;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        SetHelpText();
    }

    private void SetHelpText()
    {
        switch(priority)
        {
            case Priority.Speed: 
                helpText.text = "This turret targets SPEED. It will attack your FASTEST soldiers first.";
                break;
            case Priority.Size:
                helpText.text = "This turret targets SIZE. It will attack your LARGEST soldiers first.";
                break;
            default:
                helpText.text = "This turret targets PROXIMITY. It will attack your CLOSEST soldiers first.";
                break;
        }
    }

    private void UpdateTarget()
    {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
        if(enemies.Count == 0)
        {
            target = null;
            return;
        }

        enemies.RemoveAll((x)=>OutOfRange(transform, x.transform, radius));

        switch(priority)
        {
            case Priority.Speed: 
                enemies = SortFastestTarget(enemies);
                break;
            case Priority.Size:
                enemies = SortLargestTarget(enemies);
                break;
            default:
                enemies = SortClosestTarget(enemies);
                break;
        }

        if(enemies.Count == 0)
        {
            target = null;
            return;
        }
        else
        {
            target = enemies[0].transform;
        }

        
    }

    private static bool OutOfRange(Transform here,Transform there, float r)
    {
        Vector3 herePosition = here.position;
        herePosition.y = 0.0f;

        Vector3 therePosition = there.position;
        therePosition.y = 0.0f;
        
        return Vector3.Distance(herePosition, therePosition) > r;
    }

    private List<GameObject> SortFastestTarget(List<GameObject> enemies)
    {
        enemies.Sort((x,y)=>y.GetComponent<Soldier>().speed.CompareTo(x.GetComponent<Soldier>().speed));
        return enemies;
    }

    private List<GameObject> SortClosestTarget(List<GameObject> enemies)
    {
        enemies.Sort((x,y)=>Vector3.Distance(transform.position, x.transform.position).CompareTo(Vector3.Distance(transform.position, y.transform.position)));
        return enemies;
    }

    private List<GameObject> SortLargestTarget(List<GameObject> enemies)
    {
        enemies.Sort((x,y)=>y.GetComponent<Soldier>().size.CompareTo(x.GetComponent<Soldier>().size));
        return enemies;
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
        bullet.damage = damage;

        if (bullet != null)
        {
            bullet.Seek(target);
        }
            
        if(audioSource != null)
        {
            audioSource.PlayOneShot(gunshot);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void OnMouseEnter()
    {
        helpCanvas.SetActive(true);
    }

    void OnMouseExit()
    {
        helpCanvas.SetActive(false);
    }
}
