using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        rb.linearVelocity = velocity;

        SetupVisuals();
    }

    public void PickupItem()
    {
        if (Inventory.Instance.AddItem(itemData))
            Destroy(gameObject);
        else
            rb.linearVelocityY = 2f;
    }
    private void SetupVisuals()
    {
        if (this.itemData != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = itemData.icon;

            gameObject.name = "Item Object - " + itemData.itemName;
        }
    }
}
