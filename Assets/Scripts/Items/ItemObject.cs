using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;

        gameObject.name = "Item Object - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            //player.AddItem(itemData);
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
            Debug.Log("Picked up item: " + itemData.itemName);
        }        
    }
}
