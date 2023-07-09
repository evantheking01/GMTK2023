using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using  UnityEngine.Events;
public class Soldier : MonoBehaviour
{
    public float maxHealth = 100f;
    public GameObject deathAnimation;

    public float speed = 1f;

    public float size = 1;
    private float currentHealth;
    public HealthBar healthBar;
    [SerializeField] private Transform movePositionTransform;
    public UnityEvent<Vector3> deathEvent;

    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip footstep;

    public bool playScreamOnDeath = false;

    private AudioSource audioSource;

    private Animator animator;

    private bool isDead;

    private void Awake() 
    {   isDead = false;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("Hopping", true);

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
        {
            navMeshAgent.destination = movePositionTransform.position;
        }
    }

    public void TakeDamage(float damage)
    {
        if(isDead)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            Kill();
            StartCoroutine(DeathHelper());
        }  
    }

    public IEnumerator DeathHelper()
    {
        navMeshAgent.speed = 0;
        animator.SetBool("Hopping", false);
        
        if(playScreamOnDeath)
        {
            PlayScream();
            yield return new WaitForSeconds(scream.length);
        }
        
        deathEvent.Invoke(transform.position);
        Destroy(gameObject);
    }

    public void Kill()
    {
        currentHealth = 0;
        TakeDamage(10);

        healthBar.SetHealth(currentHealth);

        GameObject newDeathAnimation = Instantiate(deathAnimation, transform.position, transform.rotation) as GameObject;
        ParticleSystem[] newParticles = newDeathAnimation.GetComponentsInChildren<ParticleSystem>();
        float longestSoFar = 0f;
        for (int i = 0; i < newParticles.Length; i++)
        {
            var em = newParticles[i].emission;
            em.enabled = true;
            newParticles[i].Play();
            float currentParticleDuration = newParticles[i].main.duration + newParticles[i].main.startLifetime.constant;
            if(currentParticleDuration > longestSoFar)
                longestSoFar = currentParticleDuration;
        }

        Destroy(newDeathAnimation, longestSoFar);
    }

    public void PlayScream()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(scream);
            Debug.Log("SCREAM");
        }
    }

    public void PlayFootstep()
    {
        if(audioSource != null)
        {
            //audioSource.PlayOneShot(footstep);
        }
    }

    public void Move()
    {
        navMeshAgent.speed = speed;
    }

    public void StopMoving()
    {
        navMeshAgent.speed = 0;
        PlayFootstep();
    }
}
