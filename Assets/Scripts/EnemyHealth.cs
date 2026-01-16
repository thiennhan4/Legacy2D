using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("References")]
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Optional: Play hit animation
        // anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        
        // Play death animation if available
        if (anim != null)
        {
            anim.SetTrigger("Dead");
        }

        // Disable enemy
        GetComponent<Collider2D>().enabled = false;

        // Destroy after animation
        Destroy(gameObject, 1f);
    }
}
