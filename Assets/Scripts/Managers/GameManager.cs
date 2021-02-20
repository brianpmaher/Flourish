using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    /// <summary>
    /// Manages the game state and checks for win and loss conditions.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onGameWon;
        [SerializeField] private UnityEvent onGameLost;
        
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
                .Where(plant => plant.GetHealth() != PlantHealth.Dead)
                .All(plant => plant.GetHealth() == PlantHealth.CompleteHealthy);

        private bool GameLost() => _plants.All(plant => plant.GetHealth() == PlantHealth.Dead);
    }
}