using System.Collections.Generic;
using System;

[Serializable]
public class InventoryData
{
    public int count;
    public List<Item> inventory = new();

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
}
