using UnityEngine;
using UnityEngine.Events;

namespace Plant
{
    /// <summary>
    /// The surface to handle nutrient collision.
    /// </summary>
    public class NutrientSurface : MonoBehaviour
    {
        #region Unity Inspector Fields

        [SerializeField] private UnityEvent onNutrientAdded;

        #endregion
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Nutrients"))
            {
                onNutrientAdded.Invoke();
            }
        }
    }
}