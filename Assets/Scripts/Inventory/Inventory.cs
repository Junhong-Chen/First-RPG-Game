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
    public Dictionary<ItemData, InventoryItem> equipmentDictionary = new();

    [Header("UI")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject StashUI;

    private UI_ItemSlot[] itemSlotsOfInventory;
    private UI_ItemSlot[] itemSlotsOfStash;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        itemSlotsOfInventory = inventoryUI.GetComponentsInChildren<UI_ItemSlot>();
        itemSlotsOfStash = StashUI.GetComponentsInChildren<UI_ItemSlot>();

    }

    private void UpdateUI()
    {
        for (var i = 0; i < inventory.Count; i++)
        {
            itemSlotsOfInventory[i].UpdateSlot(inventory[i]);
        }

        for (var i = 0; i < stash.Count; i++)
        {
            itemSlotsOfStash[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            AddToInventory(item);
        else if (item.itemType == ItemType.Material)
            AddToStash(item);
        else
            Debug.LogError("Item type not recognized: " + item.itemType);

        UpdateUI();
    }

    private void AddToInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem itemInventory))
        {
            itemInventory.AddCount();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    private void AddToStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem itemStash))
        {
            itemStash.AddCount();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
            RemoveItemFromInventory(item);
        else if (item.itemType == ItemType.Material)
            RemoveItemfromStash(item);
        else
            Debug.LogError("Item type not recognized: " + item.itemType);

        UpdateUI();
    }

    private void RemoveItemFromInventory(ItemData item)
    {
        if (inventoryDictionary.TryGetValue(item, out InventoryItem itemInventory))
        {
            if (itemInventory.count <= 1)
            {
                inventory.Remove(itemInventory);
                inventoryDictionary.Remove(item);
            }
            else
            {
                itemInventory.RemoveCount();
            }
        }
    }

    private void RemoveItemfromStash(ItemData item)
    {
        if (stashDictionary.TryGetValue(item, out InventoryItem itemStash))
        {
            if (itemStash.count <= 1)
            {
                stash.Remove(itemStash);
                stashDictionary.Remove(item);
            }
            else
            {
                itemStash.RemoveCount();
            }
        }
    }
}
