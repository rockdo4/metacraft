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

            item.SetData(reward.data.id, reward.itemNameText.text, reward.data.iconName, reward.itemCountText.text, reward.itemCountText.text);

            count--;
            yield return wfs;
        }
    }
    void SaveItems()
    {
        var rewards = stageReward.rewards;
        var inventory = GameManager.Instance.inventoryData;

        foreach (var item in rewards)
        {
            if (!inventory.ContainsKey(item.data.id))
                inventory[item.data.id] = item.data;
            else
                inventory[item.data.id].AddCount(item.data.count);
        }
    }
}
