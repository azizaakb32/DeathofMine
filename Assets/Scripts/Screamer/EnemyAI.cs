using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Settings")]
    public float attackDistance = 2f;
    public int damage = 10;
    public float attackCooldown = 1.5f;

    private float attackTimer;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null)
            return;

        attackTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("Run", true);
            animator.SetBool("Attack", false);
        }
        else
        {
            agent.isStopped = true;

            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;

            if (lookPos != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookPos);

            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);

            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0;

                PlayerHealth hp = player.GetComponent<PlayerHealth>();

                if (hp != null)
                {
                    hp.TakeDamage(damage);
                }
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
