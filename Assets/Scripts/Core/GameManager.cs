using UnityEngine;

namespace Core
{
    public class GameManager
    {
        public static GameManager current;
        private void Awake() => current = this;

        [Header("Environment Settings")]
        public float gravityStrength;
        public bool invertGravity;

        // Functions
        public void ChangeForm(bool invertGravity)
        {
            this.invertGravity = invertGravity;
            Physics2D.gravity = new Vector2(0f, invertGravity ? gravityStrength : -gravityStrength);
        }

        // Events
        public void Start()
        {
            gravityStrength = Physics2D.gravity.magnitude;
        }
    }
}
