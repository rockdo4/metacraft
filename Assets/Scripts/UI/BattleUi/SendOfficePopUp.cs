using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendOfficePopUp : MonoBehaviour
{
    public RewardItem rewardItemPref;
    public Transform rewardTr;
    public Button checkButton;
    public List<GameObject> itemList = new();

    public void SetItems(List<RewardItem> list)
    {
        ResetItems();
        for (int i = 0; i < list.Count; i++)
        {
            var item = Instantiate(rewardItemPref, rewardTr);
            itemList.Add(item.gameObject);
            item.SetData(list[i].data);
        }
    }

    public void OnClickCheckButton()
    {
        gameObject.SetActive(false);
    }
    public void ResetItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }

        itemList.Clear();
    }
}
