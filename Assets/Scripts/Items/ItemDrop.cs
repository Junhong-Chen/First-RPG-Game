using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDropItems;
    [SerializeField] protected float dropAmount = 1;
    private List<ItemData> dropList = new();

    [SerializeField] private GameObject dropPrefab;

    public virtual void Drop()
    {
        for (int i = 0; i < possibleDropItems.Length; i++)
        {
            if (Random.Range(0f, 1f) < possibleDropItems[i].dropChance)
            {
                dropList.Add(possibleDropItems[i]);
            }
        }

        
        for (int i = 0; i < dropAmount; i++)
        {
            if (dropList.Count <= 0)
                break;

            ItemData itemData = dropList[Random.Range(0, dropList.Count - 1)];
            DropItem(itemData);
            dropList.Remove(itemData);
        }
    }

    protected void DropItem(ItemData itemData)
    {
        GameObject dropItem = Instantiate(this.dropPrefab, transform.position, Quaternion.identity);

        Vector2 velocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 15));

        dropItem.GetComponent<ItemObject>().SetupItem(itemData, velocity);
    }
}
