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

    public void AddCount()
    {
        count++;
    }

    public void RemoveCount()
    {
        count--;
    }
}
