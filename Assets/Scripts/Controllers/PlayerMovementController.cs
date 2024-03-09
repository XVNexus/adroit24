using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        // Variables
        public float moveSpeed;
        public float moveForce;
        public float jumpForce;
        private bool _isGrounded;

        // References
        public Rigidbody2D cRigidbody2D;

        // Events
        private void Start()
        {
            cRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // Get movement data
            var iHorizontal = Input.GetAxis("Horizontal");
            var iVertical = Input.GetAxis("Vertical");
            _isGrounded = Physics2D.Raycast(transform.position - new Vector3(0f, 0.51f),
                Vector2.down, 0.1f);

            // Apply movement
            var forceX = (iHorizontal * moveSpeed - cRigidbody2D.velocity.x) * moveForce * (_isGrounded ? 1f : .1f);
            var forceY = iVertical * -Physics2D.gravity.y * .6f;
            cRigidbody2D.AddForce(new Vector2(forceX, forceY));
            if (_isGrounded && iVertical > 0)
            {
                cRigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
