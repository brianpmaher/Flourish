using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the day & night cycle.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Length of time in seconds that a full day/night cycle should take")]
    private float dayLength = 30f;

    [SerializeField]
    [Tooltip("Offsets time to start the day at an expected time")]
    private float timeOffset = 0f;

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Factor to shade objects by")]
    private float shadeFactor = 0.25f;

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Factor to tint objects by")]
    private float tintFactor = 0.25f;

    private readonly Dictionary<SpriteRenderer, Color> _affectedChildrenInitialColorMap = new Dictionary<SpriteRenderer, Color>();

    private void Start()
    {
        foreach (Transform childTransform in transform)
        {
            var childGameObject = childTransform.gameObject;
            var childSpriteRenderer = childGameObject.GetComponent<SpriteRenderer>();            
            
            if (childSpriteRenderer != null)
            {
                _affectedChildrenInitialColorMap.Add(childSpriteRenderer, childSpriteRenderer.color);
            }
        }
    }

    private void Update()
    {
        foreach (var childRenderer in _affectedChildrenInitialColorMap.Keys)
        {
            var timeOfDay = Time.timeSinceLevelLoad % dayLength + timeOffset;
            var halfDay = dayLength / 2f;
            var initialColor = _affectedChildrenInitialColorMap[childRenderer];
            var nightColor = initialColor * shadeFactor;
            var dayColor = initialColor * (1f + tintFactor);
            Color color;

            // Going from night to day
            if (timeOfDay < halfDay)
            {
                var dayProgress = timeOfDay / halfDay;
                color = Color.Lerp(nightColor, dayColor, dayProgress);
            }
            // Going from day to night
            else
            {
                var nightProgress = (timeOfDay - halfDay) / halfDay;
                color = Color.Lerp(dayColor, nightColor, nightProgress);
            }

            childRenderer.color = color;
        }
    }
}
