using UnityEngine;
using UnityEngine.Assertions;

namespace Plant
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AgeSystem))]
    public class DeathSystem : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color deathColor = Color.black;

        private SpriteRenderer _spriteRenderer;
        private AgeSystem _ageSystem;
        private bool _isDead;

        public void Die()
        {
            _isDead = true;
            _spriteRenderer.sprite = sprites[_ageSystem.stage];
            _spriteRenderer.color = deathColor;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _ageSystem = GetComponent<AgeSystem>();
        }
    }
}