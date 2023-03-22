using System.Collections;
using System.Collections.Generic;
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
    }

    public IEnumerator CoSetReward()
    {
        var rewards = stageReward.rewards;

        WaitForSeconds wfs = new (0.3f);
        count = rewards.Count;

        foreach (var reward in rewards)
        {
            GameObject itemPref = Instantiate(rewardPref, rewardTr);
            RewardItem item = itemPref.GetComponent<RewardItem>();

            item.SetData(reward.data.id, reward.itemNameText.text, reward.itemCountText.text);

            count--;
            yield return wfs;
        }
    }
}
