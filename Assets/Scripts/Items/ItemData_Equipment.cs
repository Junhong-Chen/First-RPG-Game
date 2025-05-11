using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory, // 饰品
    Flask // 药水
}

[CreateAssetMenu(fileName = "ItemData_Equipment", menuName = "Scriptable Objects/ItemData_Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength; // 力量：1点力量=1点攻击力/1%暴击伤害
    public int agility; // 敏捷：1点敏捷=1%暴击率/1%闪避率
    public int intelligence; // 智力：1点智力=1点法术攻击力/1%法术抵抗
    public int vitality; // 体力：1点体力=5点生命值/1%物理抵抗

    [Header("Offensive Stats")]
    public int damage; // 伤害
    public int critDamage; // 暴击伤害
    public int critChance; // 暴击率

    [Header("Defensive Stats")]
    public int health; // 生命值
    public int armor; // 物理防御
    public int evasion; // 闪避率
    public int magicResist; // 法术抵抗

    [Header("Magic Stats")]
    public int fireDamage; // 火焰伤害
    public int iceDamage; // 冰霜伤害
    public int lightningDamage; // 雷电伤害

    [Header("Craft Requirements")]
    public List<InventoryItem> craftMaterials = new();

    [Header("Cooldown")]
    public float cooldownTime; // 冷却时间

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.ModifierAdd(strength);
        playerStats.agility.ModifierAdd(agility);
        playerStats.intelligence.ModifierAdd(intelligence);
        playerStats.vitality.ModifierAdd(vitality);

        playerStats.damage.ModifierAdd(damage);
        playerStats.critDamage.ModifierAdd(critDamage);
        playerStats.critChance.ModifierAdd(critChance);

        playerStats.maxHealth.ModifierAdd(health);
        playerStats.armor.ModifierAdd(armor);
        playerStats.evasion.ModifierAdd(evasion);
        playerStats.magicResist.ModifierAdd(magicResist);

        playerStats.fireDamage.ModifierAdd(fireDamage);
        playerStats.iceDamage.ModifierAdd(iceDamage);
        playerStats.lightningDamage.ModifierAdd(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.ModifierRemove(strength);
        playerStats.agility.ModifierRemove(agility);
        playerStats.intelligence.ModifierRemove(intelligence);
        playerStats.vitality.ModifierRemove(vitality);

        playerStats.damage.ModifierRemove(damage);
        playerStats.critDamage.ModifierRemove(critDamage);
        playerStats.critChance.ModifierRemove(critChance);

        playerStats.maxHealth.ModifierRemove(health);
        playerStats.armor.ModifierRemove(armor);
        playerStats.evasion.ModifierRemove(evasion);
        playerStats.magicResist.ModifierRemove(magicResist);

        playerStats.fireDamage.ModifierRemove(fireDamage);
        playerStats.iceDamage.ModifierRemove(iceDamage);
        playerStats.lightningDamage.ModifierRemove(lightningDamage);
    }

    public void Effects(Entity from, Entity to)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(from, to);
        }
    }
}
