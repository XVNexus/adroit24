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
            EventSystem.current.EmitPlayerFinish();
        }
    }
}
