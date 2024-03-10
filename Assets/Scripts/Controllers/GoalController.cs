using Core;
using UnityEngine;

namespace Controllers
{
    public class GoalController : MonoBehaviour
    {
        public const string Tag = "Goal";

        // Events
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Ignore collisions with non-player objects
            if (!other.CompareTag(PlayerController.Tag)) return;

            // Mark the level complete and disable player physics temporarily
            EventSystem.current.EmitPlayerFinish();
            other.GetComponent<PlayerController>().DisablePhysics();
        }
    }
}
