using Core;
using UnityEngine;

namespace Controllers
{
    public class ModifierController : MonoBehaviour
    {
        [Header("Player Modification")]
        public bool modifyPlayer;
        public Vector2 playerSize;
        public bool playerIsBall;

        [Header("Environment Modification")]
        public bool modifyEnvironment;
        public bool invertGravity;

        // Events
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Ignore collisions with non-player objects
            if (!other.CompareTag(PlayerController.Tag)) return;

            // Apply modifications
            if (modifyPlayer)
            {
                other.GetComponent<PlayerController>().ChangeForm(playerSize, playerIsBall);
            }
            if (modifyEnvironment)
            {
                LevelSystem.current.ChangeForm(invertGravity);
            }
        }
    }
}
