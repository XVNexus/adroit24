using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public const string Tag = "Player";

        [Header("Movement Settings")]
        public float moveSpeed;
        public float moveForce;
        public float jumpForce;

        [Header("Player Shape")]
        public float sizeX;
        public float sizeY;
        public bool isBall;

        [Header("Ground Contact")]
        public const int MaxFramesOnGround = 5;
        public int framesOnGround;
        public bool isOnGround;
        private int _jumpCooldown;

        [Header("Historical Variables")]
        private Vector3 _lastPosition;

        // References
        public Rigidbody2D cRigidbody2D;

        // Functions
        public void ChangeForm(float sizeX, float sizeY, bool isBall)
        {
            // Update variables with new settings
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.isBall = isBall;

            // Apply form to player object
            transform.localScale = new Vector3(sizeX, isBall ? sizeX : sizeY, 1f);
            // TODO: TRANSFORMATION BETWEEN BALL AND SQUARE
        }

        // Events
        private void Start()
        {
            // Get references to required components
            cRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            OnCollision(other);
        }

        private void OnCollision(Collision2D other)
        {
            framesOnGround = MaxFramesOnGround;
            isOnGround = true;
        }

        // Updates
        private void FixedUpdate()
        {
            // Get movement data
            var iHorizontal = Input.GetAxis("Horizontal");
            var iVertical = Input.GetAxis("Vertical");

            // Apply movement
            var forceX = (iHorizontal * moveSpeed - cRigidbody2D.velocity.x) * moveForce * (isOnGround ? 1f : .1f);
            var forceY = iVertical * Physics2D.gravity.magnitude * .6f;
            cRigidbody2D.AddForce(new Vector2(forceX, forceY));
            if (iVertical > 0 && isOnGround && _jumpCooldown == 0)
            {
                cRigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                _jumpCooldown = MaxFramesOnGround + 1;
            }

            // Update on ground and jump status
            if (framesOnGround > 0)
            {
                isOnGround = --framesOnGround > 0;
            }
            if (_jumpCooldown > 0)
            {
                _jumpCooldown--;
            }

            // Save current position for future reference
            _lastPosition = transform.position;
        }
    }
}
