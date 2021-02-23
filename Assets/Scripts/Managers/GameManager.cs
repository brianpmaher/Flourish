﻿using System.Collections.Generic;
using System.Linq;
using Plant;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    /// <summary>
    /// Manages the game state and checks for win and loss conditions.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Unity Inspector Fields

        [SerializeField] private UnityEvent onGameWon;
        [SerializeField] private UnityEvent onGameLost;
        
        #endregion
        
        private readonly List<IPlant> _plants = new List<IPlant>();
        private bool _gameOver;
    
        public void RegisterPlant(IPlant plant)
        {
            _plants.Add(plant);
        }

        private void Update()
        {
            if (_gameOver) return; // Only show game over screens once.
            
            if (GameLost())
            {
                onGameLost.Invoke();
                _gameOver = true;
            }
            else if (GameWon())
            {
                onGameWon.Invoke();
                _gameOver = true;
            }
        }

        private bool GameWon() => 
            _plants
                .Where(plant => plant.IsAlive())
                .All(plant => plant.IsHealthy() && plant.IsDoneGrowing());

        private bool GameLost() => _plants.All(plant => plant.IsDead());
    }
}