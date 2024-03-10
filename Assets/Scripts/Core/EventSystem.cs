using System;
using UnityEngine;

namespace Core
{
    public class EventSystem : MonoBehaviour
    {
        public static EventSystem current;
        private void Awake() => current = this;

        public const float PlayerTransformTime = .5f;
        public const float LevelTransitionTime = 1.5f;

        // Events
        public void EmitEnvironmentUpdate(bool invertGravity) => OnEnvironmentUpdate?.Invoke(invertGravity);
        public event Action<bool> OnEnvironmentUpdate;

        public void EmitPlayerSpawn(Vector2 position) => OnPlayerSpawn?.Invoke(position);
        public event Action<Vector2> OnPlayerSpawn;

        public void EmitPlayerDespawn() => OnPlayerDespawn?.Invoke();
        public event Action OnPlayerDespawn;

        public void EmitPlayerFinish() => OnPlayerFinish?.Invoke();
        public event Action OnPlayerFinish;
    }
}
