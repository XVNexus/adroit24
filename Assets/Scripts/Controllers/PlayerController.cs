using Core;
using Unity.Mathematics;
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
        private int _jumpCooldown;
        private Vector2 _respawnPoint;

        [Header("Player Shape")]
        public Vector2 size;
        public bool isBall;
        private float _movementScale = 1f;
        private float _movementScaleSqrt = 1f;

        [Header("References")]
        public SpriteRenderer cSquareSpriteRenderer;
        public SpriteRenderer cCircleSpriteRenderer;
        public SensorController cSensorController;
        private Rigidbody2D _cRigidbody2D;
        private BoxCollider2D _cBoxCollider2D;
        private CircleCollider2D _cCircleCollider2D;

        // Functions
        public void ChangeForm(Vector2 size, bool isBall)
        {
            // Update variables with new settings
            this.size = size;
            this.isBall = isBall;
            _movementScale = Mathf.Floor(size.magnitude / Mathf.Sqrt(2f) * 2) * .5f;
            _movementScaleSqrt = Mathf.Sqrt(_movementScale);

            // Apply form to player object
            var newScale = new Vector3(size.x * .9f, (isBall ? size.x : size.y) * .9f, 1f);
            gameObject.LeanScale(newScale, EventSystem.PlayerTransformTime).setEaseInOutCubic();
            if (isBall)
            {
                _cRigidbody2D.constraints = RigidbodyConstraints2D.None;
                _cBoxCollider2D.enabled = false;
                _cCircleCollider2D.enabled = true;
                cSquareSpriteRenderer.gameObject.LeanScale(Vector3.zero, EventSystem.PlayerTransformTime).setEaseInOutCubic();
                cCircleSpriteRenderer.gameObject.LeanScale(Vector3.one, EventSystem.PlayerTransformTime).setEaseInOutCubic();
            }
            else
            {
                _cRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.localRotation = quaternion.identity;
                _cBoxCollider2D.enabled = true;
                _cCircleCollider2D.enabled = false;
                cSquareSpriteRenderer.gameObject.LeanScale(Vector3.one, EventSystem.PlayerTransformTime).setEaseInOutCubic();
                cCircleSpriteRenderer.gameObject.LeanScale(Vector3.zero, EventSystem.PlayerTransformTime).setEaseInOutCubic();
            }
        }

        public void Respawn()
        {
            DisablePhysics();
            gameObject.LeanMove(_respawnPoint, EventSystem.LevelTransitionTime * .5f)
                .setEaseOutCubic().setOnComplete(() =>
            {
                EnablePhysics();
            });
        }

        public void EnablePhysics()
        {
            _cRigidbody2D.simulated = true;
            _cBoxCollider2D.isTrigger = false;
            _cCircleCollider2D.isTrigger = false;
        }

        public void DisablePhysics()
        {
            _cRigidbody2D.simulated = false;
            _cBoxCollider2D.isTrigger = true;
            _cCircleCollider2D.isTrigger = true;
            _cRigidbody2D.velocity = Vector2.zero;
            _cRigidbody2D.angularVelocity = 0f;
        }

        // Events
        private void Start()
        {
            // Subscribe to events
            EventSystem.current.OnPlayerSpawn += OnPlayerSpawn;
            EventSystem.current.OnPlayerDespawn += OnPlayerDespawn;

            // Get references to required components
            _cRigidbody2D = GetComponent<Rigidbody2D>();
            _cBoxCollider2D = GetComponent<BoxCollider2D>();
            _cCircleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void OnPlayerSpawn(Vector2 position)
        {
            _respawnPoint = position;
            transform.position = position - new Vector2(LevelSystem.HorizontalOffset, 0f);
            gameObject.LeanMove(position, EventSystem.LevelTransitionTime * .5f).setEaseOutCubic().setOnComplete(() =>
            {
                EnablePhysics();
            });
        }

        private void OnPlayerDespawn()
        {
            gameObject.LeanMove(transform.position + new Vector3(LevelSystem.HorizontalOffset, 0f), EventSystem.LevelTransitionTime * .5f).setEaseInCubic();
        }

        // Updates
        private void FixedUpdate()
        {
            // Get movement input
            var iHorizontal = Input.GetAxis("Horizontal");
            var iVertical = Input.GetAxis("Vertical");
            var iJump = LevelSystem.current.invertGravity ? iVertical < 0f : iVertical > 0f;
            var isOnGround = cSensorController.isTriggered;

            // Apply movement
            var gravityFactor = LevelSystem.current.invertGravity ? 1f : -1f;
            var forceX = !isBall
                ? (iHorizontal * moveSpeed * _movementScale - _cRigidbody2D.velocity.x) * moveForce * (isOnGround ? 1f : .1f)
                : iHorizontal * moveForce * .2f;
            var forceY = !isBall
                ? iVertical * Physics2D.gravity.magnitude * .5f
                : -Physics2D.gravity.y * .5f;
            _cRigidbody2D.AddForce(new Vector2(forceX * _movementScaleSqrt, forceY));
            var jump = !isBall && iJump && isOnGround && _jumpCooldown == 0;
            if (jump)
            {
                _cRigidbody2D.AddForce(new Vector2(0f, jumpForce * _movementScaleSqrt * -gravityFactor), ForceMode2D.Impulse);
                _jumpCooldown = SensorController.MaxTriggeredFrames + 1;
            }

            // Update jump status
            if (_jumpCooldown > 0)
            {
                _jumpCooldown--;
            }

            // Respawn player if out of bounds
            var bounds = LevelSystem.current.bounds;
            var playerPosition = transform.position;
            var outOfBounds = playerPosition.x < bounds.x || playerPosition.x > bounds.z
                || playerPosition.y < bounds.y || playerPosition.y > bounds.w;
            if (outOfBounds && _cRigidbody2D.simulated)
            {
                Respawn();
            }
        }
    }
}
