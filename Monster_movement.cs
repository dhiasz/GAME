using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_movement : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private Animator animator;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < chaseRange && distance > attackRange)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); // stop
            animator.SetBool("isWalking", false);
            if (Time.time > lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
                // Tambahkan damage ke player di sini
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
