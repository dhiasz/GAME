using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    // Attack
    public float timeBetweenAttacks = 1.5f;
    bool alreadyAttacked;

    // State
    public float sightRange = 10f, attackRange = 2f;
    public bool playerInSightRange, playerInAttackRange;

    // Melee Attack
    public float meleeDamage = 10f;
    public float meleeRadius = 2f;
    public LayerMask playerLayer;

    // Health
    public float health = 100f;

    private bool isDead = false;

    // Animator
    private Animator animator;

    private void Awake()
    {
        GameObject playerObj = GameObject.Find("Paladin");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player 'Paladin' not found!");
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null) Debug.LogError("NavMeshAgent not found on enemy!");
        if (animator == null) Debug.LogError("Animator not found on enemy!");
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Set animasi jalan hanya jika masih jalan ke tujuan
        if (animator != null)
        {
            bool isWalking = agent.remainingDistance > agent.stoppingDistance && agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsWalking", isWalking);
        }

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y + 10f, transform.position.z + randomZ);

        if (Physics.Raycast(potentialPoint, Vector3.down, out RaycastHit hit, 20f, whatIsGround))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // stop movement

        // Smooth look at player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // avoid tilting
        if (direction.magnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (!alreadyAttacked)
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            Invoke(nameof(ApplyMeleeDamage), 0.5f); // Delay sinkron dengan animasi

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ApplyMeleeDamage()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, meleeRadius, playerLayer);
        foreach (Collider target in hitPlayers)
        {
            target.GetComponent<healthBar>()?.SendMessage("takeDamage", meleeDamage);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (animator != null)
                animator.SetTrigger("Die");

            agent.enabled = false;
            Destroy(gameObject, 2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}