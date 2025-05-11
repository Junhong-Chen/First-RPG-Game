using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int count = 1;

    public InventoryItem(ItemData data)
    {
        this.data = data;
    }

    public void AddCount(int amount = 1)
    {
        count += amount;
    }

    public void RemoveCount(int amount = 1)
    {
        count -= amount;
    }
}
