using UnityEngine;

namespace Plant
{
    /// <summary>
    /// Ages a plant through various life stages.  "Age" refers to the lifespan within a "Stage" which describes the
    /// different appearances of the plant as it grows.  In other words, age is a subset of a stage.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(DeathSystem))]
    public class AgeSystem : MonoBehaviour
    {
        private const int Seedling = -1;
        private const float MaxAgePerStage = 100;
        private const float MinAgePerStage = 0;

        #region Unity Inspector Fields

        [SerializeField] public int stage = Seedling;
        [SerializeField] private float secondsPerStage = 40;
        [SerializeField] [Tooltip("How fast to age during the first stage")] private float seedlingAgeFactor = 2;
        [SerializeField] public Sprite[] sprites;
        
        #endregion
        
        private SpriteRenderer _spriteRenderer;
        private HealthSystem _healthSystem;
        private DeathSystem _deathSystem;
        private bool _canAge;
        private float _age;

        public bool IsSeedling => stage == Seedling;
        public bool IsFinalStage => stage == FinalStage;
        private int FinalStage => sprites.Length - 1;

        public void StartAging()
        {
            _canAge = true;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _healthSystem = GetComponent<HealthSystem>();
            _deathSystem = GetComponent<DeathSystem>();
        }

        private void Update()
        {
            UpdateAge();
            UpdateSprite();
        }

        private void UpdateAge()
        {
            // Check if we can age first
            if (!_canAge) return;
            
            // Don't age if unhealthy
            if (_healthSystem.IsUnhealthy) return;
            
            // Stop aging at final stage
            if (IsFinalStage) return;
            
            // Age differently during the seedling stage
            var ageFactor = stage == Seedling ? seedlingAgeFactor : 1;
            
            // Increment age
            _age += MaxAgePerStage / secondsPerStage * Time.deltaTime * ageFactor;

            // Check if age has progressed enough in this stage to advance another stage
            if (_age < MaxAgePerStage) return;
            stage++;
            _age = MinAgePerStage;
        }
        
        private void UpdateSprite()
        {
            // Don't render any sprites for seedlings
            if (IsSeedling) return;
            
            // Don't update the sprite if dead
            if (_deathSystem.isDead) return;

            // Get the appropriate sprite for the life stage
            _spriteRenderer.sprite = sprites[stage];
        }
    }
}