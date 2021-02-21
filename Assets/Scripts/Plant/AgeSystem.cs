using System.Collections;
using UnityEngine;

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
        public int MaxStage;
        public float SecondsPerAge = 60;
        public bool ShouldAge;

        private float _age = 0;
        private static float MinAge { get; } = 0;
        private static float MaxAge { get; } = 100;

        public AgeSystem(int initialStage)
        {
            Stage = initialStage;
        }
        
        public bool IsMaxStage() => Stage == MaxStage;

        public IEnumerator Run()
        {
            const float secondsPerUpdate = 1;
            IncreaseAge(MaxAge / SecondsPerAge * secondsPerUpdate);
            yield return new WaitForSeconds(secondsPerUpdate);
            yield return Run();
        }

        private void IncreaseAge(float age)
        {
            if (Stage == MaxStage || !ShouldAge) return;
            
            _age += age;
            if (_age >= MaxAge)
            {
                Stage++;
                _age = MinAge;
            }
        }
    }
}