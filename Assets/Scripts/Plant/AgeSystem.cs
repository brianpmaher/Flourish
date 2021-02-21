namespace Plant
{
    /// <summary>
    /// A plant age system.  An "age" is a period within a stage, where a "stage" is the larger plant age.  So if a
    /// plant has 6 stages, it will age from 0 to 100 per stage.  Once it reaches the max stage, the plant will stop
    /// aging.
    /// </summary>
    public class AgeSystem
    {
        public int Stage { get; private set; }

        private int _maxStage;
        private float _age = 0;
        private const float _minAge = 0;
        private const float _maxAge = 100;

        public AgeSystem(int initialStage, int maxStage)
        {
            Stage = initialStage;
            _maxStage = maxStage;
        }

        public static AgeSystem FromAgeSystem(AgeSystem ageSystem)
        {
            return new AgeSystem(ageSystem.Stage, ageSystem._maxStage) { _age = ageSystem._age };
        }
        
        public void SetMaxStage(int maxStage) => _maxStage = maxStage;

        public bool IsMaxStage() => Stage == _maxStage;

        public void IncreaseAge(float age)
        {
            if (Stage == _maxStage) return;
            
            _age += age;
            if (_age >= _maxAge)
            {
                Stage++;
            }
        }
    }
}