using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;
    private CharacterStats stats;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        stats = entity.GetComponentInParent<CharacterStats>();

        entity.onFlip += FlipUI; // Subscribe to the flip event

        stats.onHealthChange += UpdateHealthUI; // Subscribe to the health change event
    }

    private void OnDisable()
    {
        entity.onFlip -= FlipUI; // Unsubscribe from the flip event
        stats.onHealthChange -= UpdateHealthUI; // Unsubscribe from the health change event
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealth();
        slider.value = stats.GetHealth();
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);
}
