using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.item != null)
        {
            Inventory.Instance.AddItem(this.item.data);
            Inventory.Instance.UnequipItem(this.item.data as ItemData_Equipment);
            UpdateSlot(null);
        }
    }
}
