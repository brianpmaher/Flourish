namespace Plant
{
    public class NutrientSystem
    {
        private readonly float _maxLevel;
        private readonly float _minHealthyLevel;
        private readonly float _minLevel;
        private readonly float _maxHealthyLevel;
        private float _currentLevel;
        private readonly float _decreasePerSecond;
        private readonly float _increasePerSecond;

        public NutrientSystem(
            float minLevel,
            float minHealthyLevel,
            float maxLevel,
            float maxHealthyLevel,
            float initialLevel,
            float increasePerSecond,
            float decreasePerSecond
        )
        {
            _minLevel = minLevel;
            _minHealthyLevel = minHealthyLevel;
            _maxLevel = maxLevel;
            _maxHealthyLevel = maxHealthyLevel;
            _currentLevel = initialLevel;
            _increasePerSecond = increasePerSecond;
            _decreasePerSecond = decreasePerSecond;
        }


        public bool IsWithinRange() => _currentLevel >= _minLevel && _currentLevel <= _maxLevel;

        public bool IsHealthy() => _currentLevel >= _minHealthyLevel && _currentLevel <= _maxHealthyLevel;

        public bool IsUnhealthy() => !IsHealthy();
    }
}