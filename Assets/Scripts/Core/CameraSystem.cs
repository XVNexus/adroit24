using UnityEngine;

namespace Core
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem current;
        private void Awake() => current = this;

        [Header("Camera Settings")]
        public Vector2 viewSize;
        private float lastCameraAspect = 0f;

        [Header("References")]
        private Camera cCamera;

        // Functions
        public void ResizeView(Vector2 viewSize)
        {
            this.viewSize = viewSize;
            cCamera.orthographicSize =
                Mathf.Max(viewSize.y * .5f * (viewSize.x / viewSize.y) / cCamera.aspect, viewSize.y * .5f);
        }

        // Events
        private void Start()
        {
            cCamera = Camera.main;
        }

        // Updates
        private void Update()
        {
            // Configure the viewport to fit the screen
            if (Mathf.Approximately(cCamera.aspect, lastCameraAspect)) return;
            ResizeView(viewSize);
            lastCameraAspect = cCamera.aspect;
        }
    }
}
