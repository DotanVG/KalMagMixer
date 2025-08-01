using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite idleSprite;
        public Sprite walkSpriteA;
        public Sprite walkSpriteB;
        public Sprite jumpSprite;
        public Sprite duckSprite;
        public Sprite climbSpriteA;
        public Sprite climbSpriteB;
        public Sprite hitSprite;
        public Sprite frontSprite;

        private SpriteRenderer sr;
        private float walkAnimTimer = 0f;
        private bool walkAnimToggle = false;
        private bool isDucking = false;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = idleSprite;
        }

        public void UpdateAnimation(float moveInput, bool isGrounded)
        {
            if (isDucking && isGrounded)
            {
                sr.sprite = duckSprite;
                return;
            }

            if (!isGrounded)
            {
                sr.sprite = jumpSprite;
                return;
            }

            if (moveInput != 0)
            {
                // Walk animation
                walkAnimTimer += Time.deltaTime;
                if (walkAnimTimer > 0.15f)
                {
                    walkAnimToggle = !walkAnimToggle;
                    walkAnimTimer = 0f;
                }
                sr.sprite = walkAnimToggle ? walkSpriteA : walkSpriteB;
            }
            else
            {
                sr.sprite = idleSprite;
            }
        }

        public void Jump()
        {
            sr.sprite = jumpSprite;
        }

        public void SetDuck(bool duck)
        {
            isDucking = duck;
        }

        // More functions for climb, hit, etc. can be added here.
    }
}
