using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    public Item data;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(string id, string itemName, string iconName, string info,string sort, string dataID, string itemCount = null)
    {
        data.id = id;
        data.name = itemName;
        data.count =  itemCount == null ? 0 :int.Parse(itemCount);
        data.iconName = iconName;
        data.info = info;
        data.sort = sort;
        data.dataID = dataID;
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