using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    // Private variables
    private float moveInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isJumping = false;

    private bool isDead = false;
    private bool isFacingRight = true;

    // Public property for other scripts to access
    public bool IsDead => isDead;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        // Store previous ground state
        wasGrounded = isGrounded;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Detect landing (was in air, now on ground)
        if (!wasGrounded && isGrounded)
        {
            isJumping = false;
            anim.SetBool("IsJumping", false);
        }

        // Handle movement
        Movement();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        }


    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Don't move while attacking (optional - remove if you want to move while attacking)
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null && playerAttack.IsAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Apply movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Movement()
    {
        // Movement input
        moveInput = Input.GetAxis("Horizontal");

        // Update running animation - simplified condition
        anim.SetBool("IsRunning", Mathf.Abs(moveInput) > 0.1f && isGrounded);

        // Flip character based on movement direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        isJumping = true;
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        anim.SetBool("IsJumping", true);
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");

        // Disable player controls
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // Disable the collider
        GetComponent<Collider2D>().enabled = false;

        // Optional: Respawn or Game Over after delay
        Invoke(nameof(OnDeathComplete), 2f);
    }

    void OnDeathComplete()
    {
        // You can implement respawn logic here
        // Or call GameManager to show game over screen
        Debug.Log("Player died! Implement respawn or game over logic.");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Visualize ground check in Editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
