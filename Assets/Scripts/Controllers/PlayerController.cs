using Core;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public const string Tag = "Player";

        public const float TransformTime = .5f;

        [Header("Movement Settings")]
        public float moveSpeed;
        public float moveForce;
        public float jumpForce;

        [Header("Player Shape")]
        public Vector2 size;
        public bool isBall;

        [Header("Ground Contact")]
        public const int MaxFramesOnGround = 5;
        public int framesOnGround;
        public bool isOnGround;
        private int _jumpCooldown;

        [Header("Historical Variables")]
        private Vector3 _lastPosition;

        [Header("References")]
        public SpriteRenderer cSquareSpriteRenderer;
        public SpriteRenderer cCircleSpriteRenderer;
        private Rigidbody2D _cRigidbody2D;
        private BoxCollider2D _cBoxCollider2D;
        private CircleCollider2D _cCircleCollider2D;

        // Functions
        public void ChangeForm(Vector2 size, bool isBall)
        {
            // Update variables with new settings
            this.size = size;
            this.isBall = isBall;

            // Apply form to player object
            gameObject.LeanScale(new Vector3(size.x, isBall ? size.x : size.y, 1f), TransformTime).setEaseInOutCubic();
            if (isBall)
            {
                _cRigidbody2D.constraints = RigidbodyConstraints2D.None;
                _cBoxCollider2D.enabled = false;
                _cCircleCollider2D.enabled = true;
                cSquareSpriteRenderer.gameObject.LeanScale(Vector3.zero, TransformTime).setEaseInOutCubic();
                cCircleSpriteRenderer.gameObject.LeanScale(Vector3.one, TransformTime).setEaseInOutCubic();
            }
            else
            {
                _cRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.localRotation = quaternion.identity;
                _cBoxCollider2D.enabled = true;
                _cCircleCollider2D.enabled = false;
                cSquareSpriteRenderer.gameObject.LeanScale(Vector3.one, TransformTime).setEaseInOutCubic();
                cCircleSpriteRenderer.gameObject.LeanScale(Vector3.zero, TransformTime).setEaseInOutCubic();
            }
        }

        // Events
        private void Start()
        {
            // Subscribe to events
            EventSystem.current.OnPlayerSpawn += OnPlayerSpawn;

            // Get references to required components
            _cRigidbody2D = GetComponent<Rigidbody2D>();
            _cBoxCollider2D = GetComponent<BoxCollider2D>();
            _cCircleCollider2D = GetComponent<CircleCollider2D>();
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
            var contactPoint = other.contacts[0].point.y;
            var position = _lastPosition.y;
            var touchedGround = LevelSystem.current.invertGravity
                ? contactPoint > position
                : contactPoint < position;
            if (!touchedGround) return;
            framesOnGround = MaxFramesOnGround;
            isOnGround = true;
        }

        private void OnPlayerSpawn(Vector2 position)
        {
            transform.position = position;
        }

        // Updates
        private void FixedUpdate()
        {
            // Get movement data
            var iHorizontal = Input.GetAxis("Horizontal");
            var iVertical = Input.GetAxis("Vertical");
            var iJump = LevelSystem.current.invertGravity ? iVertical < 0f : iVertical > 0f;

            // Apply movement
            var forceX = !isBall
                ? (iHorizontal * moveSpeed - _cRigidbody2D.velocity.x) * moveForce * (isOnGround ? 1f : .1f)
                : iHorizontal * moveForce * .2f;
            var forceY = !isBall
                ? iVertical * Physics2D.gravity.magnitude * .6f
                : 0f;
            _cRigidbody2D.AddForce(new Vector2(forceX, forceY));
            var jump = !isBall && iJump && isOnGround && _jumpCooldown == 0;
            if (jump)
            {
                _cRigidbody2D.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);
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
