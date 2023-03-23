using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    public RewardItem itemPrev;
    public Transform itemTr;
    public List<RewardItem> items;
    public TextMeshProUGUI info;
    public Item nowItem;
    public Button useButton;
    public UsePopup usePopup;
    

    private void OnEnable() // Init으로 대체 예정
    {
        var inventoryData = GameManager.Instance.inventoryData.inventory;

        foreach (var item in inventoryData)
        {
            var i = Instantiate(itemPrev, itemTr);
            i.SetData(item.id, item.name, item.iconName, item.info,item.sort, item.dataID, item.count.ToString());
            items.Add(i);
            i.GetComponent<Button>().onClick.AddListener(() => SetInfo(i.data));
        }
    }

    public void OnClickAllButton()
    {
        foreach (var item in items)
            item.gameObject.SetActive(true);
    }
    public void OnClickSortButton(string sort)
    {
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

    public void SetInfo(Item item)
    {
        info.text = item.info;
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
        for (int i = 0; i < items.Count; i++)
        {

        }
    }
}
