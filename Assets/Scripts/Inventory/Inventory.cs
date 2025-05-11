using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new();

    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new();

    public List<InventoryItem> equipment = new();
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary = new();

    public List<ItemData> InitializeItems= new();

    [Header("UI")]
    [SerializeField] private GameObject UI_Inventory;
    [SerializeField] private GameObject UI_Stash;
    [SerializeField] private GameObject UI_Equipment;

    private UI_ItemSlot[] itemSlotsOfInventory;
    private UI_ItemSlot[] itemSlotsOfStash;
    private UI_EquipmentSlot[] itemSlotsOfEquipment;

    // ��ֵһ�������޴��ֵ�������һ��ʹ��ʱ�޷�����
    private float useFlaskLastTime = float.NegativeInfinity;
    private float useArmorLastTime = float.NegativeInfinity;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        itemSlotsOfInventory = UI_Inventory.GetComponentsInChildren<UI_ItemSlot>();
        itemSlotsOfStash = UI_Stash.GetComponentsInChildren<UI_ItemSlot>();
        itemSlotsOfEquipment = UI_Equipment.GetComponentsInChildren<UI_EquipmentSlot>();

        InitializeEquipment();
    }

    // ��ȡ��ʼװ��
    public void InitializeEquipment()
    {
        foreach (ItemData item in InitializeItems)
        {
            if (item is ItemData_Equipment equipmentItem)
            {
                AddItem(equipmentItem);
            }
        }
    }

    private void UpdateUI()
    {
        for (var i = 0; i < inventory.Count; i++)
        {
            itemSlotsOfInventory[i].UpdateSlot(inventory[i]);
        }
        if (inventory.Count < itemSlotsOfInventory.Length)
        {
            for (var i = inventory.Count; i < itemSlotsOfInventory.Length; i++)
            {
                itemSlotsOfInventory[i].UpdateSlot(null);
            }
        }

        for (var i = 0; i < stash.Count; i++)
        {
            itemSlotsOfStash[i].UpdateSlot(stash[i]);
        }
        if (stash.Count < itemSlotsOfStash.Length)
        {
            for (var i = stash.Count; i < itemSlotsOfStash.Length; i++)
            {
                itemSlotsOfStash[i].UpdateSlot(null);
            }
        }

        for (int i = 0; i < itemSlotsOfEquipment.Length; i++)
        {
            bool found = false;
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> equipmentItem in equipmentDictionary)
            {
                if (equipmentItem.Key.equipmentType == itemSlotsOfEquipment[i].slotType)
                {
                    itemSlotsOfEquipment[i].UpdateSlot(equipmentItem.Value);
                    found = true;
                    break;
                }
            }
            // ���û���ҵ���Ӧ��װ�����򽫸ò�λ���
            if (!found)
            {
                itemSlotsOfEquipment[i].UpdateSlot(null);
            }
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
        if (item.type == ItemType.Equipment)
        {
            if (inventory.Count >= itemSlotsOfInventory.Length)
            {
                Debug.Log("�����������޷������Ʒ: " + item.itemName);
                return false;
            }

            AddToInventory(item, amount);
        }
        else if (item.type == ItemType.Material)
            AddToStash(item, amount);
        else
            Debug.LogError("Item type not recognized: " + item.type);

        UpdateUI();

        return true;
    }

    private void AddToInventory(ItemData item, int amount = 1)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem itemInventory))
        {
            itemInventory.AddCount();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            newItem.count = amount;
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    private void AddToStash(ItemData item, int amount = 1)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem itemStash))
        {
            itemStash.AddCount(amount);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            newItem.count = amount;
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (item.type == ItemType.Equipment)
            RemoveItemFromInventory(item, amount);
        else if (item.type == ItemType.Material)
            RemoveItemfromStash(item, amount);
        else
            Debug.LogError("Item type not recognized: " + item.type);

        UpdateUI();
    }

    private void RemoveItemFromInventory(ItemData item, int amount = 1)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem itemInventory))
        {

            itemInventory.RemoveCount(amount);

            if (itemInventory.count <= 0)
            {
                inventory.Remove(itemInventory);
                inventoryDictionary.Remove(item);
            }
        }
    }

    private void RemoveItemfromStash(ItemData item, int amount = 1)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem itemStash))
        {
            itemStash.RemoveCount(amount);

            if (itemStash.count <= 0)
            {
                stash.Remove(itemStash);
                stashDictionary.Remove(item);
            }
        }
    }

    public void EquipItem(ItemData_Equipment item)
    {
        // �Ƴ�������ͬ����װ��
        ItemData_Equipment unequippedItem = GetEquipmentByType(item.equipmentType);
        if (unequippedItem != null)
        {
            UnequipItem(unequippedItem);
            AddItem(unequippedItem);
        }

        // ������װ��
        InventoryItem newItem = new InventoryItem(item);
        equipment.Add(newItem);
        equipmentDictionary.Add(item, newItem);
        item.AddModifiers(); // ���װ�����Լӳ�

        // �ӱ������Ƴ�װ��
        RemoveItem(item);
    }

    public void UnequipItem(ItemData_Equipment equip)
    {
        if (equipmentDictionary.TryGetValue(equip, out InventoryItem inventoryItem))
        {
            equipmentDictionary.Remove(equip);
            equipment.Remove(inventoryItem);
            equip.RemoveModifiers(); // �Ƴ�װ�����Լӳ�
        }

        UpdateUI();
    }

    public bool CraftItem(ItemData_Equipment item2Craft)
    {
        List<InventoryItem> requiredMaterials = item2Craft.craftMaterials;

        for (int i = 0; i < requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(requiredMaterials[i].data, out InventoryItem itemInStash))
            {
                if (itemInStash.count < requiredMaterials[i].count)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        for (var i = 0; i < requiredMaterials.Count; i++)
        {
            RemoveItem(requiredMaterials[i].data, requiredMaterials[i].count);
        }

        AddItem(item2Craft);
        Debug.Log("Crafted " + item2Craft.itemName);

        return true;
    }

    public ItemData_Equipment GetEquipmentByType(EquipmentType type)
    {
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> equipmentItem in equipmentDictionary)
        {
            if (equipmentItem.Key.equipmentType == type)
            {
                return equipmentItem.Key;
            }
        }
        return null;
    }

    public void UseFlask()
    {
        ItemData_Equipment flask = GetEquipmentByType(EquipmentType.Flask);

        if (flask != null && Time.time > flask.cooldownTime + useFlaskLastTime)
        {
            Player player = PlayerManager.instance.player;

            // �жϽ�ɫ�Ƿ�����Ѫ
            if (player.stats.GetMaxHealth() == player.stats.GetHealth())
            {
                return;
            }

            useFlaskLastTime = Time.time;

            flask.Effects(null, player);

            SkillManager.instance.onUseSkill.Invoke(SkillType.Flask);
        }

    }

    public void UseArmor()
    {
        ItemData_Equipment armor = GetEquipmentByType(EquipmentType.Armor);

        if (armor != null && Time.time > armor.cooldownTime + useArmorLastTime)
        {
            Player player = PlayerManager.instance.player;

            // �жϽ�ɫѪ���Ƿ���� 20%
            if (player.stats.GetHealth() > player.stats.GetMaxHealth() * 0.2f)
            {
                return;
            }

            useArmorLastTime = Time.time;

            armor.Effects(player, null);
        }
    }
}
