using System;
using System.Collections;
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

    [SerializeField] private float rotationRate = 5.0f;
    
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
    [SerializeField] private GameObject gunFlash;

    private AudioSource audioSource;
    
    [SerializeField] private Transform pitchTransform;
    [SerializeField] private Transform yawTransform;
    private Quaternion _startingPitch;
    private float _startingYaw;
    private float _currentYaw = 0.0f;

    [SerializeField] private float rotationSpeed = 5.0f;
    
    [SerializeField] private float fakeYawSteps = 15.0f;

    [SerializeField] private float shootThresholdAngle = 10.0f;
    [SerializeField] private bool useZrotation = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
        SetHelpText();

        if (pitchTransform != null)
            _startingPitch = pitchTransform.rotation;

        if (yawTransform != null)
        {
            _startingYaw = transform.rotation.eulerAngles.y;
            _currentYaw = _startingYaw;
        }
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
        bool isFacing = true;
        if (yawTransform != null)
        {
            float targetYaw = _startingYaw;
            if (target != null)
            {
                Vector3 from = transform.position;
                Vector3 to = target.position;

                Vector3 direction = (to - from).normalized;

                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                targetYaw = rotation.eulerAngles.y;
            }

            _currentYaw = Mathf.MoveTowardsAngle(_currentYaw, targetYaw, rotationSpeed * Time.deltaTime);
                
            if (Mathf.Abs(Mathf.DeltaAngle(_currentYaw, targetYaw)) > shootThresholdAngle)
                isFacing = false;

            float desiredYaw = _currentYaw - _startingYaw;
            if (fakeYawSteps > 0.0f)
                desiredYaw = Mathf.Round(desiredYaw / fakeYawSteps) * fakeYawSteps;

            if (useZrotation)
                yawTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, desiredYaw + 90.0f);
            else
                yawTransform.localRotation = Quaternion.Euler(0.0f, desiredYaw, 0.0f);
        }
        
        if(target != null)
        {
            // Vector3 direction = target.position - transform.position;
            // Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Vector3 rotation = Quaternion.Lerp(gun.rotation, lookRotation, Time.deltaTime).eulerAngles;
            // gun.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (gun != null)
                gun.LookAt(target);

            if(fireCountdown <= 0f && isFacing)
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

        StartCoroutine(GunFlashHelper());
    }

    private IEnumerator GunFlashHelper()
    {
        gunFlash.SetActive(true);
        
        yield return new WaitForSeconds(.1f);
        gunFlash.SetActive(false);
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
