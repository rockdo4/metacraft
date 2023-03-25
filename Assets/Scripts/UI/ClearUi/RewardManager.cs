using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public Transform rewardTr;
    public GameObject rewardPref;
    public StageReward stageReward;

    public List<GameObject> rewardList = new();

    public void SetReward(bool isLast = false)
    {
        StartCoroutine(CoSetReward());
        if (isLast)
        {
            SaveItems();
        }
    }

    public void ResetReward()
    {
        for (int i = rewardList.Count-1; i >= 0; i--)
        {
            Destroy(rewardList[i]);
        }
        rewardList.Clear();
    }

    public IEnumerator CoSetReward()
    {
        var rewards = stageReward.nowRewards;
        Logger.Debug(rewards.Count);
        WaitForSecondsRealtime wfs = new(0.3f);
        int count = rewards.Count;

        foreach (var reward in rewards)
        {
            GameObject itemPref = Instantiate(rewardPref, rewardTr);
            rewardList.Add(itemPref);
            RewardItem item = itemPref.GetComponent<RewardItem>();

            item.SetData(reward.id,
                reward.name,
                reward.iconName,
                reward.info,
                reward.sort,
                reward.dataID,
                reward.count.ToString());;

            count--;
            yield return wfs;
        }
    }
    public void SaveItems()
    {
        var rewards = stageReward.rewards;
        ref InventoryData inventoryData = ref GameManager.Instance.inventoryData;

        foreach (var item in rewards)
        {
            inventoryData.AddItem(item.data);
        }
    }
}
