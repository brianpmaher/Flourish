using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

/// <summary>
/// A pourable container that activates a particle system when tipping to pour.
/// </summary>
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(Rigidbody2D))]
public class Pourable : MonoBehaviour
{
    [SerializeField]
    [RequiredMember]
    private ParticleSystem pourParticleSystem;

    [SerializeField]
    [RequiredMember]
    private Transform spout;

    [SerializeField]
    private float pourAngle = 45f;

    private Draggable _draggable;
    private Rigidbody2D _rigidbody2D;
    private bool _tipping;
    
    private void Start()
    {
        _draggable = GetComponent<Draggable>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        var emissionModule = pourParticleSystem.emission;
        emissionModule.enabled = false;
    }

    private void Update()
    {
        if (_draggable.IsDragging && Input.GetMouseButtonDown((int) MouseButton.RightMouse))
        {
            _tipping = true;
            // TODO: Kind of a hack since the draggable freezes both to stop any rotation the object may have had.
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            transform.rotation = Quaternion.Euler(0, 0, pourAngle);
            pourParticleSystem.transform.position = spout.position;
            var emissionModule = pourParticleSystem.emission;
            emissionModule.enabled = true;
        }
        else if (_tipping)
        {
            pourParticleSystem.transform.position = spout.position;
        }
        
        if (_tipping && Input.GetMouseButtonUp((int) MouseButton.RightMouse) || _tipping && !_draggable.IsDragging)
        {
            _tipping = false;
            transform.rotation = Quaternion.identity;
            var emissionModule = pourParticleSystem.emission;
            emissionModule.enabled = false;
        }
    }
}