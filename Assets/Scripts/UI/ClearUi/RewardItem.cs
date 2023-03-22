using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Item
{
    public string id;
    public string name;
    public int count;
    public string info;
}

public class RewardItem : MonoBehaviour
{
    public Item data;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(string id, string itemName, string itemCount = null, string info = null)
    {
        data.id = id;
        data.name = itemName;
        data.count =  itemCount == null ? 0 :int.Parse(itemCount);
        itemNameText.text = itemName;
        if (itemCountText != null)
            itemCountText.text = itemCount;
    }
    public void AddCount(string count)
    {
        int nowCount = int.Parse(itemCountText.text);
        itemCountText.text = (nowCount + int.Parse(count)).ToString();
    }
}