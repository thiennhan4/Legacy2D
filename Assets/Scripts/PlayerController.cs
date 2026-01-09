using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator aim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aim = GetComponent<Animator>();
    }
    void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}
