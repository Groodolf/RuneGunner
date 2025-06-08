using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Параметры врага")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 10f;
    public float attackCooldown = 1.5f;
    public float moveSpeed = 3.5f;
    public float chaseRange = 1000f;
    public float attackRange = 2f;

    [Header("Игрок")]
    public Transform player;
    private PlayerHealth playerHealth;

    private Animator animator;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.Play("Action"); 
        isAttacking = false;
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.Play("Idle Skel"); 
        isAttacking = false;
    }

    void Attack()
    {
        agent.isStopped = true;

        if (!isAttacking)
        {
            animator.Play("Skel Attack");
            isAttacking = true;
        }

        // Атака по кулдауну
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.Play("Skel Dmg"); 
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.Play("Death"); 
        Destroy(gameObject, 3f); 
    }
}