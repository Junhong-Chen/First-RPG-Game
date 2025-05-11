using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop_Player: ItemDrop
{
    [Header("Player Drop")]
    [Range(0f, 1f)]
    [SerializeField] private float dropChanceOfEquipment;
    [Range(0f, 1f)]
    [SerializeField] private float dropChanceOfStash;

    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance;
    }

    public override void Drop()
    {
        List<InventoryItem> equipment = new List<InventoryItem>(inventory.equipment);
        List<InventoryItem> stash = new List<InventoryItem>(inventory.stash);

        foreach (InventoryItem item in equipment)
        {
            if (Random.Range(0f, 1f) < this.dropChanceOfEquipment)
            {
                DropItem(item.data);
                inventory.UnequipItem(item.data as ItemData_Equipment);
            }
        }

        foreach (InventoryItem item in stash)
        {
            if (Random.Range(0f, 1f) < this.dropChanceOfStash)
            {
                DropItem(item.data);
                inventory.RemoveItem(item.data, item.count);
            }
        }
    }
}
