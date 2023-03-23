using System.Collections.Generic;
using System;

[Serializable]
public class InventoryData
{
    public int count;
    public List<Item> inventory = new();

    public void UseItem(Item item, int count)
    {
        var idx = FindItem(item);
        if (idx == -1)
            return;
        else
            inventory[idx].DelCount(count);
    }
    public void AddItem(Item item)
    {
        var idx = FindItem(item);
        if (idx == -1)
            inventory.Add(item);
        else
            inventory[idx].AddCount(item.count);
    }

    int FindItem(Item item)
    {
        var inventoryData = GameManager.Instance.inventoryData;
        for (int i = 0; i < inventoryData.inventory.Count; i++)
        {
            if (inventoryData.inventory[i].id == item.id)
                return i;
        }
        return -1;
    }
}

[Serializable]
public struct Item
{
    public string id;
    public string name;
    public int count;
    public string info;
    public string iconName;
    public string sort;
    public string dataID;

    public void AddCount(int cnt) => count += cnt;  
    public void DelCount(int cnt) => count -= cnt;  
}
