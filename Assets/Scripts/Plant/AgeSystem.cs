using UnityEngine;
using UnityEngine.Assertions;

namespace Plant
{
    /// <summary>
    /// Ages a plant through various life stages.  "Age" refers to the lifespan within a "Stage" which describes the
    /// different appearances of the plant as it grows.  In other words, age is a subset of a stage.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class AgeSystem : MonoBehaviour
    {
        private const int Seedling = -1;
        private const float MaxAgePerStage = 100;
        private const float MinAgePerStage = 0;
        
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float secondsPerStage = 40;
        [SerializeField] [Tooltip("How fast to age during the first stage")] private float seedlingAgeFactor = 2;
        
        private SpriteRenderer _spriteRenderer;
        private int _stage = Seedling;
        private float _age;

        private int FinalStage => sprites.Length - 1;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAge();
            UpdateSprite();
        }

        private void OnValidate()
        {
            Assert.IsTrue(sprites.Length > 0, "At least one sprite is required per plant");
        }

        private void UpdateAge()
        {
            // Stop aging at final stage
            if (_stage == FinalStage) return;

            // Age differently during the seedling stage
            var ageFactor = _stage == Seedling ? seedlingAgeFactor : 1;
            
            // Increment age
            _age += MaxAgePerStage / secondsPerStage * Time.deltaTime * ageFactor;

            // Check if age has progressed enough in this stage to advance another stage
            if (_age < MaxAgePerStage) return;
            _stage++;
            _age = MinAgePerStage;
        }
        
        private void UpdateSprite()
        {
            // Don't render any sprites for seedlings
            if (_stage == Seedling) return;

            // Get the appropriate sprite for the life stage
            _spriteRenderer.sprite = sprites[_stage];
        }
    }
}