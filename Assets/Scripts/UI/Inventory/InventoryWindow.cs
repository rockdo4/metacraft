using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public RewardItem itemPrev;
    public Transform itemTr;

    private void OnEnable()
    {
        var inventoryData = GameManager.Instance.inventoryData.inventory;

        foreach (var item in inventoryData)
        {
            var i = Instantiate(itemPrev, itemTr);
            i.SetData(item.id, item.name, item.iconName, item.info,item.sort, item.dataID, item.count.ToString());
        }
    }

    public void OnClickAllButton()
    {
        var list = itemTr.GetComponentsInChildren<RewardItem>();

        foreach (var item in list)
            item.gameObject.SetActive(false);
    }
    public void OnClickSortButton(string sort)
    {
        var list = itemTr.GetComponentsInChildren<RewardItem>();

        foreach (var item in list)
        {
            if (item.data.dataID.Equals(sort))
            {
                item.gameObject.SetActive(false);
            }
            else
                item.gameObject.SetActive(true);
        }
    }
}
