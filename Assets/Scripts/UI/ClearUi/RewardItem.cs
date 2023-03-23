using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Extensions.TextPic;

public class RewardItem : MonoBehaviour
{
    public Item data;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(Item item)
    {
        data = item;
        itemNameText.text = data.name;
        if (itemCountText != null)
            itemCountText.text = data.count.ToString();
        itemImage.sprite = GameManager.Instance.GetSpriteByAddress(data.iconName);
    }
    public void SetData(Dictionary<string, object> newData, int count)
    {
        data.id = newData["ID"].ToString();
        data.name = newData["Item_Name"].ToString();
        data.count = count;
        data.iconName = newData["Icon_Name"].ToString();
        data.info = newData["Info"].ToString();
        data.sort = newData["Sort"].ToString();
        data.dataID = newData["DataID"].ToString();
        itemNameText.text = data.name;
        if (itemCountText != null)
            itemCountText.text = count.ToString();
        itemImage.sprite = GameManager.Instance.GetSpriteByAddress(data.iconName);
    }
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