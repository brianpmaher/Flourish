using System.Linq;
using Managers;
using Plant;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class Growable : MonoBehaviour, IPlant
{
    [Header("Dependencies")] 
    [SerializeField] private GameManager gameManager;
    
    [Header("Appearance")] 
    [SerializeField] private Sprite[] healthySprites;
    [SerializeField] private Sprite[] unhealthyWaterSprites;
    [SerializeField] private Color unhealthyNpkColor = new Color(247, 91, 18);
    [SerializeField] private Color deadColor = Color.black;

    [Header("Age System")] 
    [SerializeField] private float secondsPerAge = 40;

    [Header("Water System")]
    [SerializeField] private float waterMinLevel = 0;
    [SerializeField] private float waterMaxLevel = 100;
    [SerializeField] private float waterMinHealthyLevel = 33;
    [SerializeField] private float waterMaxHealthyLevel = 67;
    [SerializeField] private float waterInitialLevel = 50;
    [SerializeField] private float waterIncreasePerSecond = 3;
    [SerializeField] private float waterDecreasePerSecond = .25f;

    [Header("NPK System")]
    [SerializeField] private float npkMinLevel = 0;
    [SerializeField] private float npkMaxLevel = 100;
    [SerializeField] private float npkMinHealthyLevel = 33;
    [SerializeField] private float npkMaxHealthyLevel = 67;
    [SerializeField] private float npkInitialLevel = 50;
    [SerializeField] private float npkIncreasePerSecond = 3;
    [SerializeField] private float npkDecreasePerSecond = .25f;

    private const int Seedling = -1;
    private static readonly Color HealthyColor = Color.white;
    private SpriteRenderer _spriteRenderer;
    private AgeSystem _ageSystem;
    private NutrientSystem _waterSystem;
    private NutrientSystem _npkSystem;
    private NutrientSystem[] _nutrientSystems;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager.RegisterPlant(this);
    }

    private void OnValidate()
    {
        Assert.IsNotNull(gameManager);
        Assert.IsTrue(healthySprites.Length > 0);
        Assert.IsTrue(unhealthyWaterSprites.Length > 0);
        Assert.AreEqual(healthySprites.Length, unhealthyWaterSprites.Length);
        _ageSystem = CreateAgeSystem();
        _nutrientSystems = CreateNutrientSystems();
    }

    private void Update()
    {
        UpdateAppearance();
    }

    private void OnParticleCollision(GameObject other)
    {
        
    }
    
    public void SetNutrientSystems(NutrientSystem[] nutrientSystems)
    {
        _nutrientSystems = nutrientSystems;
    }

    public bool IsAlive() => _nutrientSystems.All(nutrientSystem => nutrientSystem.IsWithinRange());
    
    public bool IsDead() => !IsAlive();

    public bool IsHealthy() => _nutrientSystems.All(nutrientSystem => nutrientSystem.IsHealthy());
    
    public bool IsSick() => !IsHealthy();

    public bool IsSeedling() => _ageSystem.Stage == Seedling;

    public bool IsDoneGrowing() => _ageSystem.IsMaxStage();

    private AgeSystem CreateAgeSystem()
    {
        var maxStage = healthySprites.Length - 1;
        _ageSystem ??= new AgeSystem(Seedling, maxStage);
        _ageSystem.SetMaxStage(maxStage);
        return AgeSystem.FromAgeSystem(_ageSystem);
    }
    
    private NutrientSystem CreateWaterNutrientSystem()
    {
        _waterSystem = new NutrientSystem(
            waterMinLevel, 
            waterMinHealthyLevel, 
            waterMaxLevel, 
            waterMaxHealthyLevel,
            waterInitialLevel, 
            waterIncreasePerSecond, 
            waterDecreasePerSecond
        );

        return _waterSystem;
    }
    
    private NutrientSystem CreateNpkNutrientSystem()
    {
        _npkSystem = new NutrientSystem(
            npkMinLevel, 
            npkMinHealthyLevel, 
            npkMaxLevel, 
            npkMaxHealthyLevel,
            npkInitialLevel, 
            npkIncreasePerSecond, 
            npkDecreasePerSecond
        );

        return _npkSystem;
    }

    private NutrientSystem[] CreateNutrientSystems() {
        return new[] { CreateWaterNutrientSystem(), CreateNpkNutrientSystem() };
    }
    
    private void UpdateAppearance()
    {
        _spriteRenderer.color = GetColor();
        _spriteRenderer.sprite = GetSprite();
    }
    
    private Color GetColor()
    {
        if (IsDead()) return deadColor;
        if (_npkSystem.IsUnhealthy()) return unhealthyNpkColor;
        return HealthyColor;
    }

    private Sprite GetSprite()
    {
        var stage = _ageSystem.Stage;
        if (IsDead() || _waterSystem.IsUnhealthy()) return unhealthyWaterSprites[stage];
        return healthySprites[stage];
    }
}
