using System;
using UnityEngine;

namespace Core
{
    public class EventSystem : MonoBehaviour
    {
        public static EventSystem current;
        private void Awake() => current = this;

        // Events
        public void EmitPlayerSpawn(Vector2 position) => OnPlayerSpawn?.Invoke(position);
        public event Action<Vector2> OnPlayerSpawn;

        public void EmitPlayerFinish() => OnPlayerFinish?.Invoke();
        public event Action OnPlayerFinish;

        public void EmitPlayerDie() => OnPlayerDie?.Invoke();
        public event Action OnPlayerDie;
    }
}
