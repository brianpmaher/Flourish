using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Plant
{
    /// <summary>
    /// Handles plant water levels.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AgeSystem))]
    public class WaterSystem : MonoBehaviour
    {
        #region Unity Inspector Fields
        
        [SerializeField] private Sprite[] wiltedSprites;
        [SerializeField] private float minWaterLevel;
        [SerializeField] private float maxWaterLevel = 100;
        [SerializeField] private float minHealthyWaterLevel = 33;
        [SerializeField] private float maxHealthyWaterLevel = 67;
        [SerializeField] private float waterIncreasePerSecond = 3;
        [SerializeField] private float waterDecreasePerSecond = .25f;
        [SerializeField] private float waterLevel = 50;
        [SerializeField] private float waterDecreaseAfterWateringDelaySeconds = 1;
        [FormerlySerializedAs("OnDeath")] [SerializeField] private UnityEvent onDeath;
        
        #endregion
        
        private SpriteRenderer _spriteRenderer;
        private AgeSystem _ageSystem;
        private DateTime _lastWaterTime;

        public bool IsHealthy => !IsWilted;
        private bool IsWilted => waterLevel < minHealthyWaterLevel || waterLevel > maxHealthyWaterLevel;
        private bool IsDead => waterLevel < minWaterLevel || waterLevel > maxWaterLevel;
        private bool IsWatering => 
            DateTime.Now - _lastWaterTime < TimeSpan.FromSeconds(waterDecreaseAfterWateringDelaySeconds);
        
        public void HandleWatered()
        {
            _lastWaterTime = DateTime.Now;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _ageSystem = GetComponent<AgeSystem>();
        }

        private void Update()
        {
            if (IsDead) return;
            UpdateWaterLevel();
            CheckForDeath();
        }

        private void LateUpdate()
        {
            if (IsDead) return;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            if (IsWilted)
            {
                _spriteRenderer.sprite = wiltedSprites[_ageSystem.stage];
            }
        }

        private void UpdateWaterLevel()
        {
            if (_ageSystem.IsSeedling) return;
            
            if (IsWatering)
            {
                waterLevel += waterIncreasePerSecond * Time.deltaTime;
            }
            else
            {
                waterLevel -= waterDecreasePerSecond * Time.deltaTime;
            }
        }

        private void CheckForDeath()
        {
            if (IsDead)
            {
                onDeath.Invoke();
            }
        }
    }
}