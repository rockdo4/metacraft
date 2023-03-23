using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    private int count;

    public Transform rewardTr;
    public GameObject rewardPref;
    public StageReward stageReward;

    public void SetReward()
    {
        StartCoroutine(CoSetReward());
        SaveItems();
    }

    public IEnumerator CoSetReward()
    {
        var rewards = stageReward.rewards;

        WaitForSeconds wfs = new(0.3f);
        count = rewards.Count;

        foreach (var reward in rewards)
        {
            GameObject itemPref = Instantiate(rewardPref, rewardTr);
            RewardItem item = itemPref.GetComponent<RewardItem>();

            item.SetData(reward.data.id,
                reward.itemNameText.text,
                reward.data.iconName,
                reward.data.info,
                reward.data.sort,
                reward.data.dataID,
                reward.itemCountText.text);;

            count--;
            yield return wfs;
        }
    }
    void SaveItems()
    {
        var rewards = stageReward.rewards;
        var inventoryData = GameManager.Instance.inventoryData;

        foreach (var item in rewards)
        {
            var idx = FindItem(item);
            if (idx == -1)
                inventoryData.inventory.Add(item.data);
            else
                inventoryData.inventory[idx].AddCount(item.data.count);
        }
    }
    int FindItem(RewardItem item)
    {
        var inventoryData = GameManager.Instance.inventoryData;
        for(int i = 0; i < inventoryData.inventory.Count; i++)
        {
            if (inventoryData.inventory[i].id == item.data.id)
                return i;
        }
        return -1;
    }
}
