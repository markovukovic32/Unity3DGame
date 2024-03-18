using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Transform target;
    private Animator zombieAnimator; // Reference to the Animator component
    private float detectionRange = 3f; // Range to detect player
    private bool isAttacking = false; // Indicates if the zombie is currently attacking
    private float attackDamage = 5f;

    void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        zombieAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        MoveToTarget(target);

        if (IsPlayerWithinRange() && !isAttacking)
        {
            zombieAnimator.SetBool("AttackTime", true);
            isAttacking = true;
            GameObject healthBarObject = GameObject.FindWithTag("HealthBarPlayer");
            if (healthBarObject != null)
            {
                HealthBar healthBar = healthBarObject.GetComponent<HealthBar>();
                if (healthBar != null)
                    healthBar.takeDamage(attackDamage);
                else
                    Debug.LogError("HealthBar script not found on the GameObject with the 'HealthBar' tag!");
            }
            StartCoroutine(ResetAttackAnimation());
        }
    }

    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(zombieAnimator.GetCurrentAnimatorClipInfo(0).Length);
        zombieAnimator.SetBool("AttackTime", false);
        isAttacking = false;
    }

    public void MoveToTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }

    private bool IsPlayerWithinRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        return distanceToPlayer <= detectionRange;
    }
}