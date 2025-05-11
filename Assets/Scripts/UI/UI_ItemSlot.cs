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
        if (item != null) // �����¶���Ϊ��ʱ�����²�λ
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
        else if (this.item != null) // �����¶����Ϊ�գ��ҵ�ǰ��λ����Ʒʱ����ղ�λ
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
