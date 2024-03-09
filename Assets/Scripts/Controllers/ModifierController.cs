using Core;
using UnityEngine;

namespace Controllers
{
    public class ModifierController : MonoBehaviour
    {
        public const string Tag = "Modifier";

        [Header("Player Modification")]
        public bool modifyPlayer;
        public float playerSizeX;
        public float playerSizeY;
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
                other.GetComponent<PlayerController>().ChangeForm(playerSizeX, playerSizeY, playerIsBall);
            }
            if (modifyEnvironment)
            {
                GameManager.current.ChangeForm(invertGravity);
            }
        }
    }
}
