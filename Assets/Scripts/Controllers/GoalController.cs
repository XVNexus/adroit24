using Core;
using UnityEngine;

namespace Controllers
{
    public class GoalController : MonoBehaviour
    {
        [Header("References")]
        public ParticleSystem cParticleSystem;

        // Events
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(PlayerController.Tag)) return;
            EventSystem.current.EmitPlayerFinish();
            other.GetComponent<PlayerController>().DisablePhysics();
            cParticleSystem.Play();
        }
    }
}
