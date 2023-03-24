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
                reward.data.name,
                reward.data.iconName,
                reward.data.info,
                reward.data.sort,
                reward.data.dataID,
                reward.data.count.ToString());;

            count--;
            yield return wfs;
        }
    }
    void SaveItems()
    {
        var rewards = stageReward.rewards;
        ref InventoryData inventoryData = ref GameManager.Instance.inventoryData;

        foreach (var item in rewards)
        {
            inventoryData.AddItem(item.data);
        }
    }
}
