using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem item)
    {
        if (item != null) // 当更新对象不为空时，更新槽位
        {
            itemImage.sprite = item.data.icon;
            itemImage.color = Color.white;

            if (item.count > 1)
            {
                itemCount.text = item.count.ToString();
            }
            else
            {
                itemCount.text = "";
            }
        }
        else if (this.item != null) // 当更新对象的为空，且当前槽位有物品时，清空槽位
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemCount.text = "";
        }

        this.item = item;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (this.item != null)
        {
            if (this.item.data.type == ItemType.Equipment)
            {
                Inventory.Instance.EquipItem(this.item.data as ItemData_Equipment);
            }
            //else if (item.data.itemType == ItemType.Material)
            //{
            //    Inventory.Instance.UseItem(item);
            //}
            else
            {
                Debug.LogError("Item type not recognized: " + this.item.data.type);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("show");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hide");
    }
}
