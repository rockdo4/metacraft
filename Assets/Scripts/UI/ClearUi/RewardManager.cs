using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    private int count;

    public Transform rewardTr;
    public GameObject rewardPref;

    public List<string> rewardTable;

    public void SetReward()
    {
        var currentMission = GameManager.Instance.currentSelectMission;
        int rewardRangeMin = (int)currentMission["RewardRangeMin"];
        int rewardRangeMax = (int)currentMission["RewardRangeMax"];
        count = Random.Range(rewardRangeMin, rewardRangeMax + 1);
        StartCoroutine(CoSetReward());
    }

    public IEnumerator CoSetReward()
    {
        WaitForSeconds wfs = new (0.3f);

        {
            GameObject reward = Instantiate(rewardPref, rewardTr);
            RewardItem item = reward.GetComponent<RewardItem>();
            int rewardGold = (int)GameManager.Instance.currentSelectMission["Compensation"];
            item.SetData(-1,"골드", $"{rewardGold}");// 임시로 id 정함
            GameManager.Instance.playerData.gold += rewardGold;
        }

        while (count != 0)
        {
            GameObject reward = Instantiate(rewardPref, rewardTr);
            RewardItem item = reward.GetComponent<RewardItem>();

            var idx = Random.Range(0, rewardTable.Count); // 임시로 id 정함
            item.SetData(idx,rewardTable[idx],
                $"x{Random.Range(1, 10)}");

            count--;
            yield return wfs;
        }
    }
}
