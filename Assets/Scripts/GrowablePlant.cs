using System.Collections;
using System.Linq;
using Managers;
using Plant;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class GrowablePlant : MonoBehaviour, IPlant
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
        InitSprite();
        gameManager.RegisterPlant(this);
        // TODO: Only set this once the plant is watered for the first time.
        _ageSystem.ShouldAge = true;
        StartCoroutine("RunAgeSystem");
        StartCoroutine("RunWaterSystem");
        StartCoroutine("RunNpkSystem");
    }

    private void OnValidate()
    {
        Assert.IsNotNull(gameManager);
        Assert.IsTrue(healthySprites.Length > 0);
        Assert.IsTrue(unhealthyWaterSprites.Length > 0);
        Assert.AreEqual(healthySprites.Length, unhealthyWaterSprites.Length);
        ConfigAgeSystem();
        ConfigNutrientSystems();
    }

    private void Update()
    {
        UpdateAppearance();
    }

    private void OnParticleCollision(GameObject other)
    {
        
    }

    public bool IsAlive() => _nutrientSystems.All(nutrientSystem => nutrientSystem.IsWithinRange());
    
    public bool IsDead() => !IsAlive();

    public bool IsHealthy() => _nutrientSystems.All(nutrientSystem => nutrientSystem.IsHealthy());
    
    public bool IsSick() => !IsHealthy();

    public bool IsSeedling() => _ageSystem.Stage == Seedling;

    public bool IsDoneGrowing() => _ageSystem.IsMaxStage();

    private void InitSprite()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void ConfigAgeSystem()
    {
        var maxStage = healthySprites.Length - 1;
        _ageSystem ??= new AgeSystem(Seedling);
        _ageSystem.MaxStage = maxStage;
        _ageSystem.SecondsPerAge = secondsPerAge;
    }
    
    private void ConfigWaterSystem()
    {
        _waterSystem ??= new NutrientSystem(waterInitialLevel);
        _waterSystem.MinLevel = waterMinLevel;
        _waterSystem.MaxLevel = waterMaxLevel;
        _waterSystem.MinHealthyLevel = waterMinHealthyLevel;
        _waterSystem.MaxHealthyLevel = waterMaxHealthyLevel;
        _waterSystem.DecreasePerSecond = waterDecreasePerSecond;
        _waterSystem.IncreasePerSecond = waterIncreasePerSecond;
    }
    
    private void ConfigNpkSystem()
    {
        _npkSystem ??= new NutrientSystem(npkInitialLevel);
        _npkSystem.MinLevel = npkMinLevel;
        _npkSystem.MaxLevel = npkMaxLevel;
        _npkSystem.MinHealthyLevel = npkMinHealthyLevel;
        _npkSystem.MaxHealthyLevel = npkMaxHealthyLevel;
        _npkSystem.DecreasePerSecond = npkDecreasePerSecond;
        _npkSystem.IncreasePerSecond = npkIncreasePerSecond;
    }

    private void ConfigNutrientSystems()
    {
        ConfigWaterSystem();
        ConfigNpkSystem();
        _nutrientSystems ??= new[] {_waterSystem, _npkSystem};
    }

    private IEnumerator RunAgeSystem()
    {
        yield return _ageSystem.Run();
    }

    private void RunWaterSystem()
    {
    }

    private void RunNpkSystem()
    {
    }

    private void UpdateAgeSystem()
    {
        _ageSystem.ShouldAge = IsHealthy();
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
        if (IsSeedling()) return null;
        var stage = _ageSystem.Stage;
        if (IsDead() || _waterSystem.IsUnhealthy()) return unhealthyWaterSprites[stage];
        return healthySprites[stage];
    }
}
