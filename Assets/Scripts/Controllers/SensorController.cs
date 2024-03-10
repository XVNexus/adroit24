using Core;
using UnityEngine;

namespace Controllers
{
    public class SensorController : MonoBehaviour
    {
        public const int MaxTriggeredFrames = 5;

        [Header("Sensor Status")]
        public bool isTriggered;
        private int _triggeredFrames;

        [Header("References")]
        private BoxCollider2D _cGroundBoxCollider2D;

        // Events
        private void Start()
        {
            // Subscribe to events
            EventSystem.current.OnEnvironmentUpdate += OnEnvironmentUpdate;

            // Get references to required components
            _cGroundBoxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTrigger(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTrigger(other);
        }

        private void OnTrigger(Collider2D other)
        {
            if (other.isTrigger) return;
            _triggeredFrames = MaxTriggeredFrames;
            isTriggered = true;
        }

        private void OnEnvironmentUpdate(bool invertGravity)
        {
            transform.localPosition = new Vector3(0f, transform.localPosition.magnitude * (invertGravity ? 1f : -1f));
        }

        // Updates
        private void FixedUpdate()
        {
            if (_triggeredFrames > 0)
            {
                isTriggered = --_triggeredFrames > 0;
            }
        }
    }
}
