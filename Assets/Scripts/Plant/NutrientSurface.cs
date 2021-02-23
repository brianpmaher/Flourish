using UnityEngine;
using UnityEngine.Events;

namespace Plant
{
    public class NutrientSurface : MonoBehaviour
    {
        [SerializeField] private UnityEvent onNutrientAdded;
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Nutrients"))
            {
                onNutrientAdded.Invoke();
            }
        }
    }
}