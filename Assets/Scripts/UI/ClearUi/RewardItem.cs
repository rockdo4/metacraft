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
    public string iconName;

    public void AddCount(int cnt) => count += cnt;
}

public class RewardItem : MonoBehaviour
{
    public Item data;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(string id, string itemName, string iconName, string info, string itemCount = null)
    {
        data.id = id;
        data.name = itemName;
        data.count =  itemCount == null ? 0 :int.Parse(itemCount);
        data.iconName = iconName;
        data.info = info;
        itemNameText.text = itemName;
        if (itemCountText != null)
            itemCountText.text = itemCount;
        itemImage.sprite = GameManager.Instance.GetSpriteByAddress(data.iconName);
    }
    public void AddCount(string count)
    {
        int nowCount = int.Parse(itemCountText.text);
        itemCountText.text = (nowCount + int.Parse(count)).ToString();
    }
}