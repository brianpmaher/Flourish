using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Plant
{
    /// <summary>
    /// Handles water collision surface.
    /// </summary>
    public class WateringSurface : MonoBehaviour
    {
        #region Unity Inspector Fields
        
        [FormerlySerializedAs("OnWatered")] [SerializeField] private UnityEvent onWatered;
        
        #endregion
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Water"))
            {
                onWatered.Invoke();
            }
        }
    }
}