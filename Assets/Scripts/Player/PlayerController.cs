using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float jumpForce = 8f;
        public Vector2 spawnPosition = new Vector2(-8f, -2f);

        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private bool isGrounded = false;
        private float moveInput;

        private PlayerAnimation playerAnim;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            playerAnim = GetComponent<PlayerAnimation>();
            transform.position = spawnPosition;
        }

        void Update()
        {
            HandleInput();
            playerAnim?.UpdateAnimation(moveInput, isGrounded);
        }

        void HandleInput()
        {
            moveInput = Input.GetAxisRaw("Horizontal");

            // Movement
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // Flip sprite
            Vector3 scale = transform.localScale;
            if (moveInput > 0) scale.x = 1;
            else if (moveInput < 0) scale.x = -1;
            scale.y = 1; // Force Y to always stay 1 (no upside-down)
            scale.z = 1;
            transform.localScale = scale;

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isGrounded = false;
                playerAnim?.Jump();
            }

            // Duck
            playerAnim?.SetDuck(Input.GetKey(KeyCode.DownArrow) && isGrounded);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }

        public void Respawn()
        {
            transform.position = spawnPosition;
            // Optional: Zero out velocity
            if (rb != null)
                rb.velocity = Vector2.zero;
        }

    }
}
