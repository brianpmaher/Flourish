namespace Plant
{
    public class NutrientSystem
    {
        public float MinLevel = 0;
        public float MaxLevel = 100;
        public float MinHealthyLevel = 33;
        public float MaxHealthyLevel = 67;
        public float DecreasePerSecond = .25f;
        public float IncreasePerSecond = 3;
        
        private float _currentLevel;

        public NutrientSystem(float initialLevel)
        {
            _currentLevel = initialLevel;
        }

        public bool IsWithinRange() => _currentLevel >= MinLevel && _currentLevel <= MaxLevel;

        public bool IsHealthy() => _currentLevel >= MinHealthyLevel && _currentLevel <= MaxHealthyLevel;

        public bool IsUnhealthy() => !IsHealthy();
    }
}