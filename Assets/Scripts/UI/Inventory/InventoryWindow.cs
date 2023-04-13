using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : View
{
    public RewardItem itemPrev;
    public Transform itemTr;
    public List<RewardItem> items;
    public TextMeshProUGUI itemname;
    public TextMeshProUGUI info;
    public Item nowItem;
    public Button useButton;
    public UsePopup usePopup;

    public string nowSort = string.Empty;

    private void OnEnable()
    {
        nowSort = string.Empty;
        InvnetoryClear();
    }
    private void InvnetoryClear() 
    {
        itemname.text = string.Empty;
        info.text = string.Empty;
        useButton.interactable = false;
        nowItem = null;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();

        var inventoryData =  GameManager.Instance.inventoryData.inventory;

        foreach (var item in inventoryData)
        {
            var i = Instantiate(itemPrev, itemTr);
            i.SetData(item.id, item.name, item.iconName, item.info, item.sort, item.dataID, item.count.ToString());
            items.Add(i);
            i.GetComponent<Button>().onClick.AddListener(() => SetInfo(i.data));
        }
    }

    public void OnClickAllButton()
    {
        nowSort = string.Empty;
        itemname.text = string.Empty;
        info.text = string.Empty;
        useButton.interactable = false;
        nowItem = null;
        foreach (var item in items)
            item.gameObject.SetActive(true);
    }
    public void OnClickSortButton(string sort)
    {
        nowSort = sort;
        itemname.text = string.Empty;
        info.text = string.Empty;
        useButton.interactable = false;
        nowItem = null;
        foreach (var item in items)
        {
            if (item.data.sort.Equals(sort))
            {
                item.gameObject.SetActive(true);
            }
            else
                item.gameObject.SetActive(false);
        }
    }
    public void OnDisable()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
        usePopup.getPopUp.OnClickCancle();
        usePopup.OnClickCancle();
    }

    public void SetInfo(Item item)
    {
        itemname.text = GameManager.Instance.GetStringByTable(item.name);
        info.text = GameManager.Instance.GetStringByTable(item.info);
        useButton.interactable = !item.dataID.Equals(string.Empty);
        nowItem = item;
    }

    public void OnClickUse()
    {
        usePopup.gameObject.SetActive(true);
        usePopup.Set(nowItem);
    }

    public void OnClickGetButton()
    {
        usePopup.getPopUp.OnClickCancle();
        usePopup.OnClickCancle();
        InvnetoryClear();

        if (nowSort == string.Empty)
            OnClickAllButton();
        else
            OnClickSortButton(nowSort);
    }
}
