using UnityEngine;

namespace Plant
{
    [RequireComponent(typeof(WaterSystem))]
    [RequireComponent(typeof(NutrientSystem))]
    public class HealthSystem : MonoBehaviour
    {
        private WaterSystem _waterSystem;
        private NutrientSystem _nutrientSystem;

        public bool IsHealthy => _waterSystem.IsHealthy && _nutrientSystem.IsHealthy;
        public bool IsUnhealthy => !IsHealthy;

        private void Awake()
        {
            _waterSystem = GetComponent<WaterSystem>();
            _nutrientSystem = GetComponent<NutrientSystem>();
        }
    }
}