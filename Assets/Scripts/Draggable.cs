using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A script to make an object draggable with the mouse.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Draggable : MonoBehaviour
{
    [SerializeField] private bool disableColliderOnDrag;
    
    private Camera _mainCamera;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private int _initialSortOrder;
    private bool _dragging;
    public bool IsDragging => _dragging;

    private void Start()
    {
        _mainCamera = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialSortOrder = _spriteRenderer.sortingOrder;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
        {
            _dragging = true;
            _rigidbody2D.isKinematic = true;
            transform.rotation = Quaternion.identity;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            
            if (disableColliderOnDrag)
            {
                _spriteRenderer.sortingOrder = 10;
                _collider2D.enabled = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (_dragging && Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
        {
            _dragging = false;
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            
            if (disableColliderOnDrag)
            {
                _spriteRenderer.sortingOrder = _initialSortOrder;
                _collider2D.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (_dragging)
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
}