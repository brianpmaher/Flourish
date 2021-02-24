using Managers;
using UnityEngine;

namespace Plant
{
    /// <summary>
    /// Monitors multiple systems and reports plant status.
    /// </summary>
    [RequireComponent(typeof(DeathSystem))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(AgeSystem))]
    public class PlantStatusReporter : MonoBehaviour, IPlantStatus
    {
        [SerializeField] private GameManager gameManager;

        private DeathSystem _deathSystem;
        private HealthSystem _healthSystem;
        private AgeSystem _ageSystem;
        
        public void Awake()
        {
            _deathSystem = GetComponent<DeathSystem>();
            _healthSystem = GetComponent<HealthSystem>();
            _ageSystem = GetComponent<AgeSystem>();
        }

        private void Start()
        {
            gameManager.RegisterPlantStatus(this);
        }

        public bool IsDead()
        {
            return _deathSystem.isDead;
        }

        public bool IsAlive()
        {
            return !IsDead();
        }

        public bool IsHealthy()
        {
            return _healthSystem.IsHealthy;
        }

        public bool IsDoneGrowing()
        {
            return _ageSystem.IsFinalStage;
        }
    }
}