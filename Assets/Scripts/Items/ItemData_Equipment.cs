using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory, // ��Ʒ
    Flask // ҩˮ
}

[CreateAssetMenu(fileName = "ItemData_Equipment", menuName = "Scriptable Objects/ItemData_Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength; // ������1������=1�㹥����/1%�����˺�
    public int agility; // ���ݣ�1������=1%������/1%������
    public int intelligence; // ������1������=1�㷨��������/1%�����ֿ�
    public int vitality; // ������1������=5������ֵ/1%����ֿ�

    [Header("Offensive Stats")]
    public int damage; // �˺�
    public int critDamage; // �����˺�
    public int critChance; // ������

    [Header("Defensive Stats")]
    public int health; // ����ֵ
    public int armor; // �������
    public int evasion; // ������
    public int magicResist; // �����ֿ�

    [Header("Magic Stats")]
    public int fireDamage; // �����˺�
    public int iceDamage; // ��˪�˺�
    public int lightningDamage; // �׵��˺�

    [Header("Craft Requirements")]
    public List<InventoryItem> craftMaterials = new();

    [Header("Cooldown")]
    public float cooldownTime; // ��ȴʱ��

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
