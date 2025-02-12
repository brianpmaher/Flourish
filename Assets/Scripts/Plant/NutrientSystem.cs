﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Plant
{
    /// <summary>
    /// Handles plant nutrient levels.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AgeSystem))]
    [RequireComponent(typeof(DeathSystem))]
    public class NutrientSystem : MonoBehaviour
    {
        #region Unity Inspector Fields
        
        [SerializeField] private Color unhealthyNutrientColor = new Color(247, 91, 18);
        [SerializeField] private float minNutrientLevel;
        [SerializeField] private float maxNutrientLevel = 100;
        [SerializeField] private float minHealthyNutrientLevel = 33;
        [SerializeField] private float maxHealthyNutrientLevel = 67;
        [SerializeField] private float nutrientIncreasePerSecond = 3;
        [SerializeField] private float nutrientDecreasePerSecond = .25f;
        [SerializeField] private float nutrientLevel = 50;
        [SerializeField] private float nutrientDecreaseAfterNutrientAddedDelaySeconds = 1;
        [SerializeField] private UnityEvent onDeath;
        
        #endregion
        
        private SpriteRenderer _spriteRenderer;
        private AgeSystem _ageSystem;
        private DeathSystem _deathSystem;
        private DateTime _lastNutrientTime;

        public bool IsHealthy => !IsUnhealthy;
        private bool IsUnhealthy => nutrientLevel < minHealthyNutrientLevel || nutrientLevel > maxHealthyNutrientLevel;
        private bool IsNutrientDeath => nutrientLevel < minNutrientLevel || nutrientLevel > maxNutrientLevel;
        private bool IsAddingNutrients => 
            DateTime.Now - _lastNutrientTime < TimeSpan.FromSeconds(nutrientDecreaseAfterNutrientAddedDelaySeconds);
        
        public void HandleNutrientAdded()
        {
            _lastNutrientTime = DateTime.Now;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _ageSystem = GetComponent<AgeSystem>();
            _deathSystem = GetComponent<DeathSystem>();
        }

        private void Update()
        {
            if (_deathSystem.isDead) return;
            UpdateNutrientLevel();
            CheckForDeath();
        }

        private void LateUpdate()
        {
            if (_deathSystem.isDead) return;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (IsUnhealthy)
            {
                _spriteRenderer.color = unhealthyNutrientColor;
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }

        private void UpdateNutrientLevel()
        {
            if (_ageSystem.IsSeedling) return;
            
            if (IsAddingNutrients)
            {
                nutrientLevel += nutrientIncreasePerSecond * Time.deltaTime;
            }
            else
            {
                nutrientLevel -= nutrientDecreasePerSecond * Time.deltaTime;
            }
        }

        private void CheckForDeath()
        {
            if (IsNutrientDeath)
            {
                onDeath.Invoke();
            }
        }
    }
}