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

        // --- New for anti-stuck ---
        private float jumpTimer = 0f;
        private bool inJump = false;
        private const float MaxJumpDuration = 5f;

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

            // --- Anti-stuck logic (player stuck in jump animation) ---
            if (!isGrounded && Mathf.Abs(rb.velocity.y) > 0.01f)
            {
                if (!inJump)
                {
                    jumpTimer = 0f;
                    inJump = true;
                }
                jumpTimer += Time.deltaTime;
                if (jumpTimer > MaxJumpDuration)
                {
                    Respawn();
                    jumpTimer = 0f;
                }
            }
            else
            {
                jumpTimer = 0f;
                inJump = false;
            }

            // --- Respawn on R key ---
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
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
            scale.y = 1;
            scale.z = 1;
            transform.localScale = scale;

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isGrounded = false;
                playerAnim?.Jump();
                AudioManager.Instance.PlayJumpSFX();
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

            if (collision.collider != null &&
                collision.collider.CompareTag("RespawnBlock") &&
                collision.collider.enabled)
            {
                Respawn();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("RespawnBlock"))
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            transform.position = spawnPosition;
            if (rb != null)
                rb.velocity = Vector2.zero;
        }

        // --- NEW: Static utility for GameManager ---
        /// <summary>
        /// Checks if the player is overlapping any enabled RespawnBlock colliders.
        /// If so, immediately respawn.
        /// Call after enabling new obstacles.
        /// </summary>
        public static void TryForceRespawnIfOverlapping()
        {
            var player = FindObjectOfType<PlayerController>();
            if (player == null) return;

            Collider2D playerCol = player.GetComponent<Collider2D>();
            if (playerCol == null) return;

            // Find all enabled RespawnBlock colliders
            var allBlocks = GameObject.FindGameObjectsWithTag("RespawnBlock");
            foreach (var obj in allBlocks)
            {
                Collider2D blockCol = obj.GetComponent<Collider2D>();
                if (blockCol != null && blockCol.enabled && playerCol.bounds.Intersects(blockCol.bounds))
                {
                    player.Respawn();
                    return;
                }
            }
        }
    }
}
