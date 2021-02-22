using System;
using UnityEngine;
using UnityEngine.Events;

namespace Plant
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AgeSystem))]
    public class WaterSystem : MonoBehaviour
    {
        [SerializeField] private Sprite[] wiltedSprites;
        [SerializeField] private float minWaterLevel = 0;
        [SerializeField] private float maxWaterLevel = 100;
        [SerializeField] private float minHealthyWaterLevel = 33;
        [SerializeField] private float maxHealthyWaterLevel = 67;
        [SerializeField] private float waterIncreasePerSecond = 3;
        [SerializeField] private float waterDecreasePerSecond = .25f;
        [SerializeField] private float waterLevel = 50;
        [SerializeField] private float waterDecreaseAfterWateringDelaySeconds = 1;
        [SerializeField] private UnityEvent OnDeath;
        
        private SpriteRenderer _spriteRenderer;
        private AgeSystem _ageSystem;
        private DateTime _lastWaterTime;

        private bool IsWilted => waterLevel < minHealthyWaterLevel || waterLevel > maxHealthyWaterLevel;
        private bool IsDead => waterLevel < minWaterLevel || waterLevel > maxWaterLevel;
        private bool IsWatering => 
            DateTime.Now - _lastWaterTime < TimeSpan.FromSeconds(waterDecreaseAfterWateringDelaySeconds);
        
        public void HandleWatered()
        {
            _lastWaterTime = DateTime.Now;
            
            // Kick off aging system if it hasn't started yet
            if (!_ageSystem.canAge)
            {
                _ageSystem.canAge = true;
            }
        }
        
        private void Start()
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

        // Late update to always run after AgeSystem sets the sprite
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
                OnDeath.Invoke();
            }
        }
    }
}