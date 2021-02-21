namespace Plant
{
    public interface IPlant
    {
        public bool IsAlive();
        
        public bool IsDead();
        
        public bool IsHealthy();
        
        public bool IsSick();

        public bool IsSeedling();
        
        public bool IsDoneGrowing();
    }
}