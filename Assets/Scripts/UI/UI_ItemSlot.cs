using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;

    public InventoryItem item;

    public void UpdateSlot(InventoryItem item)
    {
        this.item = item;

        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked on item slot: " + item.data.itemName);
    }
}
