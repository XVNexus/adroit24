using UnityEngine;

namespace Core
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem current;
        private void Awake() => current = this;

        public const float HorizontalOffset = 50f;
        public const float VerticalOffset = 25f;

        [Header("Level Info")]
        public Vector4 bounds;
        public GameObject[] levelPrefabs;
        private int _currentLevelIndex = -1;
        private GameObject _currentLevel;
        private bool _isLevelLoaded;

        [Header("Environment Settings")]
        public float gravityStrength;
        public bool invertGravity;

        // Functions
        public void SwitchLevel(int index)
        {
            EventSystem.current.EmitPlayerDespawn();
            _currentLevel.LeanMove(new Vector3(0f, VerticalOffset), EventSystem.LevelTransitionTime * .5f).setEaseInQuint().setOnComplete(() =>
            {
                UnloadLevel();
                LoadLevel(index);
                _currentLevel.transform.position = new Vector3(0f, -VerticalOffset);
                _currentLevel.LeanMove(Vector3.zero, EventSystem.LevelTransitionTime * .5f).setEaseOutQuint();
            });
        }

        public bool LoadLevel(int index)
        {
            if (index < 0 || index >= levelPrefabs.Length) return false;
            _currentLevel = Instantiate(levelPrefabs[index]);
            _currentLevelIndex = index;
            _isLevelLoaded = true;
            return true;
        }

        public bool UnloadLevel()
        {
            if (!_isLevelLoaded) return false;
            Destroy(_currentLevel);
            _currentLevel = null;
            _currentLevelIndex = -1;
            _isLevelLoaded = false;
            return true;
        }

        public void ChangeForm(bool invertGravity)
        {
            this.invertGravity = invertGravity;
            Physics2D.gravity = new Vector2(0f, invertGravity ? gravityStrength : -gravityStrength);
            EventSystem.current.EmitEnvironmentUpdate(invertGravity);
        }

        // Events
        private void Start()
        {
            // Subscribe to events
            EventSystem.current.OnPlayerFinish += OnPlayerFinish;

            // Set up variables
            gravityStrength = Physics2D.gravity.magnitude;

            // Load the first level to start the game
            LoadLevel(0);
        }

        private void OnPlayerFinish()
        {
            SwitchLevel(_currentLevelIndex + 1);
        }
    }
}
