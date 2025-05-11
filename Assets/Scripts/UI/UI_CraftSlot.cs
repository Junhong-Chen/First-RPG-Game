using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(this.item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftItem = this.item.data as ItemData_Equipment;

        if (craftItem != null)
        {
            Inventory.Instance.CraftItem(craftItem);
        }
    }
}
