using UnityEngine;
using UnityEngine.Serialization;

namespace Plant
{
    /// <summary>
    /// Handles plant death.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AgeSystem))]
    public class DeathSystem : MonoBehaviour
    {
        #region Unity Inspector Fields

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color deathColor = Color.black;
        
        #endregion

        private SpriteRenderer _spriteRenderer;
        private AgeSystem _ageSystem;
        public bool isDead;

        public void Die()
        {
            isDead = true;
            _spriteRenderer.sprite = sprites[_ageSystem.stage];
            _spriteRenderer.color = deathColor;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _ageSystem = GetComponent<AgeSystem>();
        }
    }
}