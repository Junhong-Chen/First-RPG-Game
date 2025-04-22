using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Flask
}

[CreateAssetMenu(fileName = "ItemData_Equipment", menuName = "Scriptable Objects/ItemData_Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
}
