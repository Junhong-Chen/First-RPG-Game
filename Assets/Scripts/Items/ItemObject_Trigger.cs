using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && !player.stats.isDead)
        {
            itemObject.PickupItem();
        }
    }
}
