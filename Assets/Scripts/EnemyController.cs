using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator _animator;

    private NavMeshAgent _navMeshAgent;

    private GameObject Player;

    public float AttackDistance = 10.0f;

    public float FollowDistance = 20.0f;

    [Range(0.0f, 1.0f)]
    public float AttackProbability = 0.5f;

    [Range(0.0f, 1.0f)]
    public float HitAccuracy = 0.5f;

    public float DamagePoints = 2.0f;

    public AudioClip GunSound = null;
    bool shoot = false;


    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();
        InvokeRepeating("ShootEvent", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Player.transform.position, this.transform.position);
        shoot = false;
        bool follow = (dist < FollowDistance);

        if (follow)
        {
            float random = Random.Range(0.0f, 1.0f);
            if (random > (1.0f - AttackProbability) && dist < AttackDistance)
            {
                shoot = true;
            }
        }
        Debug.Log("Follow: " + follow);
        if (follow)
        {
            Debug.Log("Okej");
            _navMeshAgent.SetDestination(Player.transform.position);
        }

        if (!follow || shoot)
            _navMeshAgent.SetDestination(transform.position);


        _animator.SetBool("Shoot", shoot);
        _animator.SetBool("Run", follow);


    }

    public void ShootEvent()
    {
        /*if (m_Audio != null)
        {
            m_Audio.PlayOneShot(GunSound);
        }*/
        if (!shoot)
            return;
        float random = Random.Range(0.0f, 1.0f);

        // The higher the accuracy is, the more likely the player will be hit
        bool isHit = random > 1.0f - HitAccuracy;

        if (isHit)
        {
            GameObject healthBarObject = GameObject.FindWithTag("HealthBarPlayer");
            if (healthBarObject != null)
            {
                HealthBar healthBar = healthBarObject.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.takeDamage(DamagePoints);
                }
                else
                {
                    Debug.LogError("HealthBar script not found on the GameObject with the 'HealthBar' tag!");
                }
            }
        }
    }

    public void Die()
    {
        /*if (m_Audio != null)
        {
            m_Audio.pitch = Time.timeScale;
            m_Audio.PlayOneShot(DeathSound);
        }*/

        _navMeshAgent.enabled = false;

        _animator.SetBool("IsFollow", false);
        _animator.SetBool("Attack", false);

        _animator.SetTrigger("Die");

        Destroy(gameObject);

    }
}
