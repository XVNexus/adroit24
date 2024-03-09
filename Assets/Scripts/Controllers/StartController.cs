using Core;
using UnityEngine;

namespace Controllers
{
    public class StartController : MonoBehaviour
    {
        public const string Tag = "Start";

        // Events
        private void Start()
        {
            EventSystem.current.EmitPlayerSpawn(transform.position);
        }
    }
}
