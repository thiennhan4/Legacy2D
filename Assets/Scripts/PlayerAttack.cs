using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public int attackDamage = 10;

    [Header("References")]
    [SerializeField] private Animator anim;

    // Private variables
    private bool canAttack = true;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Check if player is dead (get from PlayerController)
        PlayerController playerController = GetComponent<PlayerController>();
      //  if (playerController != null && playerController.IsDead) return;

        // Attack - Only when left mouse button is CLICKED (not held)
        if (Input.GetMouseButtonDown(0) && canAttack && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;
        canAttack = false;
        anim.SetTrigger("Attack");

        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Apply damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyHealth>() != null)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            }
        }

        // Reset attack after cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
    }

    // Call this method from Animation Event at the END of Attack animation
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        canAttack = true;
    }

    // Visualize attack range in Editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
