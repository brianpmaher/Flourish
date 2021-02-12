using System;
using System.Diagnostics;
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
        [SerializeField] private Sprite[] healthyFlowerPlantSprites;
        [SerializeField] private float secondsPerAge = 120f;

        private SpriteRenderer _spriteRenderer;
        private PlantType _plantType;
        private SpriteRenderer _plantSpriteRenderer;
        private const int SEEDLING = -1;
        private const int MAXStage = 5;
        private int _stage = -1;
        private const float MAXAge = 100f;
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
            var potHeight = _spriteRenderer.size.y;
            plantGameObject.transform.localPosition = Vector2.zero + new Vector2(0, potHeight);
            _plantSpriteRenderer = plantGameObject.AddComponent<SpriteRenderer>();
            _plantSpriteRenderer.sprite = GetPlantSprite();
            _plantSpriteRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
            _plantSpriteRenderer.sortingOrder = 1;
        }

        private Sprite GetPlantSprite()
        {
            if (_stage == SEEDLING)
            {
                return null;
            }
            
            if (_plantType == PlantType.Flower)
            {
                if (isHealthy)
                {
                    return healthyFlowerPlantSprites[_stage];
                }
            }

            throw new Exception("Unable to get plant sprite");
        }

        private void Age()
        {
            if (_stage < MAXStage)
            {
                _age += MAXAge / secondsPerAge * Time.deltaTime;

                if (_age >= MAXAge)
                {
                    _age = 0;
                    _stage += 1;
                    _plantSpriteRenderer.sprite = GetPlantSprite();
                }
            }
        }
    }
}