using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Slider healthSlider;

    [Header("Skill")]
    private SkillManager skills;
    [SerializeField] private Image blackholeCooldownImage;
    [SerializeField] private Image crystalCooldownImage;
    [SerializeField] private Image dashCooldownImage;
    [SerializeField] private Image parryCooldownImage;
    [SerializeField] private Image swordCooldownImage;
    [SerializeField] private Image flaskCooldownImage;
    [SerializeField] private ItemData_Equipment flaskData;

    [Header("Souls")]
    [SerializeField] TextMeshProUGUI soulsCount;

    void Start()
    {
        skills = SkillManager.instance;

        stats.onHealthChange += UpdateHealthUI; // Subscribe to the health change event

        skills.onUseSkill += UpdateSkillStatus;

        UpdateSoulsStatus();
        stats.soul.OnValueChanged += UpdateSoulsStatus;
    }

    private void Update()
    {
        UpdateCooldownOf(blackholeCooldownImage, skills.blackhole.cooldown);
        UpdateCooldownOf(crystalCooldownImage, skills.crystal.cooldown);
        UpdateCooldownOf(dashCooldownImage, skills.dash.cooldown);
        UpdateCooldownOf(parryCooldownImage, skills.parry.cooldown);
        UpdateCooldownOf(swordCooldownImage, skills.sword.cooldown);
        UpdateCooldownOf(flaskCooldownImage, flaskData.cooldownTime);
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = stats.GetMaxHealth();
        healthSlider.value = stats.GetHealth();
    }

    private void UpdateSkillStatus(SkillType type)
    {
        switch (type)
        {
            case SkillType.Blackhole:
                blackholeCooldownImage.fillAmount = 1;
                break;
            case SkillType.Crystal:
                crystalCooldownImage.fillAmount = 1;
                break;
            case SkillType.Dash:
                dashCooldownImage.fillAmount = 1;
                break;
            case SkillType.Parry:
                parryCooldownImage.fillAmount = 1;
                break;
            case SkillType.Sword:
                swordCooldownImage.fillAmount = 1;
                break;
            case SkillType.Flask:
                flaskCooldownImage.fillAmount = 1;
                break;
            
        }
    }

    private void UpdateSoulsStatus()
    {
        soulsCount.text = stats.soul.GetValue().ToString();
    }

    private void UpdateCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
}
