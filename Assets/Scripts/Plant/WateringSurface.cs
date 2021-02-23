using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Plant
{
    public class WateringSurface : MonoBehaviour
    {
        [FormerlySerializedAs("OnWatered")] [SerializeField] private UnityEvent onWatered;
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Water"))
            {
                onWatered.Invoke();
            }
        }
    }
}