using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plant
{
    /// <summary>
    /// A growable plant.  This script is intended to be attached to a pot, and the plant grows from the pot.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class GrowablePlant : MonoBehaviour
    {
        [SerializeField] private Sprite[] healthyFlowerSprites;
        [SerializeField] private Sprite[] wiltedFlowerSprites;
        [SerializeField] private float secondsPerAge = 120f;
        [SerializeField] private float waterDecreasePerSecond = 0.25f;
        [SerializeField] private float waterIncreasePerParticle = 0.1f;
        [SerializeField] private float nutrientDecreasePerSecond = 0.25f;
        [SerializeField] private float nutrientIncreasePerParticle = 0.1f;
        [SerializeField] private Color lowOrHighNutrientColor;
        [SerializeField] private Color deadColor;

        private SpriteRenderer _spriteRenderer;
        private PlantType _plantType;
        private SpriteRenderer _plantSpriteRenderer;
        private bool _mirrored;
        private const int Seedling = -1;
        private const int MaxStage = 5;
        private int _stage = -1;
        private const float UnitsPerAge = 100f;
        private float _age = 0f;
        private float _waterLevel = 50f;
        private float _nutrientLevel = 50f;
        private bool _canStartGrowing;

        private bool IsHealthy =>
            _waterLevel >= 33 && _waterLevel <= 67 && _nutrientLevel >= 33 && _nutrientLevel <= 67;
        private bool IsOverOrUnderWatered => _waterLevel < 33 || _waterLevel > 67;
        private bool IsOverOrUnderNutriented => _nutrientLevel < 33 || _nutrientLevel > 67;
        private bool IsDead => _waterLevel <= 0 || _waterLevel >= 100 || _nutrientLevel <= 0 || _nutrientLevel >= 100;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            InitializePlant();
        }

        private void Update()
        {
            Age();
            EvaporateWater();
            AbsorbNutrients();
            UpdatePlantSprite();
        }
        
        private void OnParticleCollision(GameObject other)
        {
            if (IsDead) return;
            
            if (_stage == Seedling)
            {
                _canStartGrowing = true;
                return;
            }

            if (other.CompareTag("Water")) _waterLevel += waterIncreasePerParticle;
            if (other.CompareTag("Nutrients")) _nutrientLevel += nutrientIncreasePerParticle;
        }

        /// <summary>
        /// Initializes the plant in this script to a random plant type and places it within the container.
        /// </summary>
        private void InitializePlant()
        {
            var plantTypes = (PlantType[]) Enum.GetValues(typeof(PlantType));
            _plantType = (PlantType) Random.Range(0, plantTypes.Length - 1);
            var plantGameObject = new GameObject(_plantType.ToString());
            plantGameObject.transform.parent = transform;
            plantGameObject.transform.localScale = Vector3.one;
            var sprite = _spriteRenderer.sprite;
            var potHeight = sprite.rect.height / sprite.pixelsPerUnit;
            plantGameObject.transform.localPosition = Vector2.zero + new Vector2(0, potHeight);
            
            _plantSpriteRenderer = plantGameObject.AddComponent<SpriteRenderer>();
            _plantSpriteRenderer.sprite = GetPlantSprite();
            _plantSpriteRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
            _plantSpriteRenderer.sortingOrder = 1;
            
            // Cheap way to add variation to sprites
            _mirrored = Random.Range(0, 2) == 1;
            _plantSpriteRenderer.flipX = _mirrored;
        }

        /// <summary>
        /// Gets the appropriate plant sprite for this plant based on the type of plant it is and its nutrient and water
        /// levels.
        /// </summary>
        /// <returns>The plant sprite to use for the stage of this plant.</returns>
        /// <exception cref="Exception">
        /// Developer error for if unable to get the plant sprite based on conditions.
        /// </exception>
        private Sprite GetPlantSprite()
        {
            if (_stage == Seedling)
            {
                return null;
            }
            
            if (_plantType == PlantType.Flower)
            {
                if (IsOverOrUnderWatered) return wiltedFlowerSprites[_stage];
                return healthyFlowerSprites[_stage];
            }

            throw new Exception("Unable to get plant sprite");
        }

        /// <summary>
        /// Updates this plant to the appropriate sprite.
        /// </summary>
        private void UpdatePlantSprite()
        {
            _plantSpriteRenderer.sprite = GetPlantSprite();
            if (IsDead) _plantSpriteRenderer.color = deadColor;
            if (IsOverOrUnderNutriented) _plantSpriteRenderer.color = lowOrHighNutrientColor;
            else _plantSpriteRenderer.color = Color.white;
        }

        /// <summary>
        /// Plant aging system.  Aging should only occur if the plant has been initially watered and had nutrients added
        /// and will only continue to age while the plant is healthy.  Once the plant reaches the max age stage, it will
        /// stop aging.
        /// </summary>
        private void Age()
        {
            if (IsDead) return;
            if (!_canStartGrowing) return;
            if (!IsHealthy) return;
            if (_stage >= MaxStage) return;
            
            _age += UnitsPerAge / secondsPerAge * Time.deltaTime;
            if (_age < UnitsPerAge) return;
            
            _age = 0;
            _stage += 1;
        }

        /// <summary>
        /// Evaporates water level over time
        /// </summary>
        private void EvaporateWater()
        {
            if (IsDead) return;
            if (_stage == Seedling) return;
            _waterLevel -= waterDecreasePerSecond * Time.deltaTime;
        }
        
        /// <summary>
        /// Decreases nutrient level over time
        /// </summary>
        private void AbsorbNutrients()
        {
            if (IsDead) return;
            if (_stage == Seedling) return;
            _nutrientLevel -= nutrientDecreasePerSecond * Time.deltaTime;
        }
    }
}