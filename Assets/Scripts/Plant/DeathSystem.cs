using UnityEngine;

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
        private bool _isDead;

        public void Die()
        {
            _isDead = true;
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