using Core;
using UnityEngine;

namespace Controllers
{
    public class StartController : MonoBehaviour
    {
        [Header("References")]
        public ParticleSystem cParticleSystem;

        // Events
        private void Start()
        {
            EventSystem.current.EmitPlayerSpawn(transform.localPosition);
            cParticleSystem.transform.localPosition = new Vector3(0f, LevelSystem.VerticalOffset);
            cParticleSystem.Play();
        }
    }
}
