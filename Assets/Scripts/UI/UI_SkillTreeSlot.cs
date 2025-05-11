using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isUnlocked;

    private Image skillIcon;

    [SerializeField] private int skillId;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor = Color.gray;

    [SerializeField] private int skillCost = 1; // Cost in souls to unlock the skill

    [SerializeField] private UI_SkillTreeSlot parentUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] conflictingSiblingsUnlocked;

    [SerializeField] private UI_Tooltip_Skill tooltip;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot - " + skillName;
    }

    void Start()
    {
        skillIcon = GetComponent<Image>();

        skillIcon.color = lockedColor; // Default color for locked state

        GetComponent<Button>().onClick.AddListener(UnlockSkill);
    }

    public void UnlockSkill()
    {
        if (isUnlocked) return;

        if (parentUnlocked != null && !parentUnlocked.isUnlocked)
        {
            Debug.LogWarning($"Cannot unlock {skillName} because parent {parentUnlocked.skillName} is not unlocked.");
            return;
        }

        foreach (var sibling in conflictingSiblingsUnlocked)
        {
            if (sibling.isUnlocked)
            {
                Debug.LogWarning($"Cannot unlock {skillName} because conflicting sibling {sibling.skillName} is already unlocked.");
                return;
            }
        }

        if (DeductSkillCost())
        {
            isUnlocked = true;
            skillIcon.color = Color.white; // Change color to indicate unlocked state
            SkillManager.instance.onUnlockSkill?.Invoke(skillId, true);
        }

    }

    private bool DeductSkillCost()
    {
        PlayerStats playerStats = PlayerManager.instance.player.stats as PlayerStats;
        int soul = playerStats.soul.GetValue();

        if (soul < skillCost)
            return false;
        else
            playerStats.soul.SetValue(soul - skillCost);

        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(new string[] { skillName, skillDescription });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}
