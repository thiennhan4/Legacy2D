using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Animator aim;
    public LayerMask gorundCheckLayer;  
    public bool isGrounded ;
    public Transform groundCheck;
    public float GroundCheckRadius = 0.2f;
    public bool isFacingRight = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJumpt();
        UpdateAnimtion();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }
    private void HandleJumpt()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded =Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, gorundCheckLayer);
    }

    private void UpdateAnimtion()
    {
        bool IsRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool IsJumping = !isGrounded;
        aim.SetBool("isRunning", IsRunning);
        
        aim.SetBool("isJumping", IsJumping);
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
       Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, GroundCheckRadius);
    }
}


