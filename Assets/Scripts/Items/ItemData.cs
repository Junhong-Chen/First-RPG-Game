using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public ItemType type;
    
    public Sprite icon;

    [Range(0f, 1f)]
    public float dropChance;
}
