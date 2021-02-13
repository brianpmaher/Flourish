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

        private bool isHealthy =>
            _waterLevel >= 40 && _waterLevel <= 60 && _nutrientLevel >= 40 && _nutrientLevel <= 60;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            InitializePlant();
        }

        private void Update()
        {
            Age();
        }

        private void InitializePlant()
        {
            var plantTypes = (PlantType[]) Enum.GetValues(typeof(PlantType));
            _plantType = (PlantType) Random.Range(0, plantTypes.Length - 1);
            var plantGameObject = new GameObject(_plantType.ToString());
            plantGameObject.transform.parent = transform;
            var potHeight = _spriteRenderer.sprite.rect.height / _spriteRenderer.sprite.pixelsPerUnit;
            plantGameObject.transform.localPosition = Vector2.zero + new Vector2(0, potHeight);
            
            _plantSpriteRenderer = plantGameObject.AddComponent<SpriteRenderer>();
            _plantSpriteRenderer.sprite = GetPlantSprite();
            _plantSpriteRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
            _plantSpriteRenderer.sortingOrder = 1;
            
            // Cheap way to add variation to sprites
            _mirrored = Random.Range(0, 2) == 1;
            _plantSpriteRenderer.flipX = _mirrored;
        }

        private Sprite GetPlantSprite()
        {
            if (_stage == Seedling)
            {
                return null;
            }
            
            if (_plantType == PlantType.Flower)
            {
                if (isHealthy)
                {
                    return healthyFlowerSprites[_stage];
                }
            }

            throw new Exception("Unable to get plant sprite");
        }

        private void Age()
        {
            if (_stage >= MaxStage) return;
            
            _age += UnitsPerAge / secondsPerAge * Time.deltaTime;

            if (_age < UnitsPerAge) return;
            
            _age = 0;
            _stage += 1;
            _plantSpriteRenderer.sprite = GetPlantSprite();
        }
    }
}