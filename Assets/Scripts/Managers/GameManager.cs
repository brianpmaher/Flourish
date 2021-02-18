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
    
        public void RegisterPlant(IPlant plant)
        {
            _plants.Add(plant);
        }

        private void Update()
        {
            if (GameWon())
            {
                onGameWon.Invoke();
            }
            else if (GameLost())
            {
                onGameLost.Invoke();
            }
        }

        private bool GameWon() => 
            _plants
                .Where(plant => plant.GetHealth() != PlantHealth.Dead)
                .Count(plant => plant.GetHealth() == PlantHealth.CompleteHealthy) > 0;

        private bool GameLost() => _plants.All(plant => plant.GetHealth() == PlantHealth.Dead);
    }
}