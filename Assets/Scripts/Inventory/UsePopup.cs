using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsePopup : MonoBehaviour
{
    public RewardItem itemPrev;
    public Transform itemTr;
    public Item nowItem;
    public TextMeshProUGUI info;
    public GetPopUp getPopUp;
    public Item useItem;
    public List<RewardItem> items = new();
    public Button useButton;


    public void OnClickCancle()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
        gameObject.SetActive(false);
    }

    public void OnClickGet()
    {
        getPopUp.gameObject.SetActive(true);

        var box = GameManager.Instance.inventoryData.inventory.Find(t => t.id.Equals(useItem.id));
        var boxCount = box.count;

        getPopUp.Set(boxCount, nowItem.count, nowItem, useItem);
    }

    public void Set(Item item)
    {
        useButton.interactable = false;
        info.text = string.Empty;
        useItem = item;
        nowItem = null;
        var data = GameManager.Instance.itemBoxList.Find(t => t["ID"].ToString().Equals(item.dataID));
        var itemData = GameManager.Instance.itemInfoList;

        var nowBoxID = item.dataID;
        string id = "ItemID ";
        string value = "ItemValue ";
        
        // for (int i = 0; i < 100; i++)
        {
            //var nowItemID = data[$"{id}{i + 1}"];
            //if (nowItemID.Equals(string.Empty))
            //    break;

            //var nowItemValue = data[$"{value}{i + 1}"];
            var nowItemID = data[$"{id}1"];
            var nowItemValue = data[$"{value}1"];

            var addItem = Instantiate(itemPrev, itemTr);
            var addItemData = itemData.Find(t => t["ID"].ToString().Equals(nowItemID.ToString()));

            addItem.GetComponent<Button>().onClick.AddListener(() => SetInfo(addItem.data));
            addItem.SetData(addItemData, int.Parse(nowItemValue.ToString()));
            items.Add(addItem);
        }
    }

    public void SetInfo(Item item)
    {
        useButton.interactable = true;
        info.text = GameManager.Instance.GetStringByTable(item.info);
        nowItem = item;
    }
}
