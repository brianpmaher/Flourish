using UnityEngine;
using UnityEngine.Events;

namespace Plant
{
    public class WateringSurface : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnWatered;
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Water"))
            {
                OnWatered.Invoke();
            }
        }
    }
}