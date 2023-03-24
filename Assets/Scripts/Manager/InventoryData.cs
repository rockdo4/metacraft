using System.Collections.Generic;
using System;

[Serializable]
public class InventoryData
{
    public int count;
    public List<Item> inventory = new();

    public void UseItem(string id, int count)
    {
        var idx = FindItemIndexCheckCount(id, count);
        if (idx == -1)
            return;
        else
        {
            if (inventory[idx].count == count)
            {
                inventory.RemoveAt(idx);
            }
            else
                inventory[idx].DelCount(count);
        }
    }

    public void AddItem(Item item)
    {
        if (item.id.CompareTo("60300001") == 0)
        {
            GameManager.Instance.playerData.gold += item.count;
            {
                return;
            }
        }
        var idx = FindItemIndex(item.id);
        if (idx == -1)
            inventory.Add(item);
        else
            inventory[idx].AddCount(item.count);
    }

    public void AddItem(string id, int count = 1)
    {
        if (id.CompareTo("60300001") == 0)
        {
            GameManager.Instance.playerData.gold += count;
            {
                return;
            }
        }
        var idx = FindItemIndex(id);
        if (idx == -1)
        {
            var data = GameManager.Instance.itemInfoList.Find(t => t["ID"].ToString().CompareTo(id) == 0);
            Item newItem = new Item(data);
            newItem.count = count;
            inventory.Add(newItem);
        }
        else
            inventory[idx].AddCount(count);
    }

    // 아이템 인덱스 찾기
    public int FindItemIndex(string id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == id)
                return i;
        }
        return -1;
    }

    // 아이템 인덱스 찾고 원하는 개수만큼 있는지 확인
    public int FindItemIndexCheckCount(string id, int count)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].id == id)
            {
                if (inventory[i].count < count)
                    return -1;
                else
                    return i;
            }
        }
        return -1;
    }

    // 아이템이 있는지 확인
    public bool IsItem(string id)
    {
        return inventory[FindItemIndex(id)] != null;
    }
    //특정 개수만큼 아이템이 있는지 확인
    public bool IsItem(string id, int count)
    {
        var idx = FindItemIndex(id);
        if (idx == -1)
            return false;

        var item = inventory[FindItemIndex(id)];
        return (item != null) && (item.count >= count);
    }
    //아이디로만 아이템 가져오기. 없으면 null
    public Item GetItem(string id)
    {
        return inventory[FindItemIndex(id)];
    }
}

[Serializable]
public class Item
{
    public string id;
    public string name;
    public int count;
    public string info;
    public string iconName;
    public string sort;
    public string dataID;

    public Item() { }
    public Item(string id, string name, int count, string info, string iconName, string sort, string dataID)
    {
        this.id = id;
        this.name = name;
        this.count = count;
        this.info = info;
        this.iconName = iconName;
        this.sort = sort;
        this.dataID = dataID;
    }
    public Item(Dictionary<string, object> data)
    {
        this.id = data["ID"].ToString();
        this.name = data["Item_Name"].ToString();
        this.count = 0;
        this.info = data["Info"].ToString();
        this.iconName = data["Icon_Name"].ToString();
        this.sort = data["Sort"].ToString();
        this.dataID = data["DataID"].ToString();
    }
    public void AddCount(int cnt)
    {
        count += cnt;
    }
    public void DelCount(int cnt)
    {
        count -= cnt;
    }
}
