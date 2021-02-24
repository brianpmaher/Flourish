namespace Plant
{
    public interface IPlantStatus
    {
        public bool IsDead();
        
        public bool IsAlive();

        public bool IsHealthy();
        
        public bool IsDoneGrowing();
    }
}