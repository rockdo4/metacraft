using System.Collections.Generic;
using UnityEngine;

public class StageReward : MonoBehaviour
{
    public GameObject rewardPage;

    public RewardItem item;
    public Transform rewardTr;
    public List<RewardItem> rewards;
    public int golds = 0;

    public string goldId;

    /************************* Minu *************************/
    public GameObject eventRewardPage;
    public Transform eventRewardTr;
    private List<RewardItem> currRewards = new();

    private void OnDisable()
    {
        rewardPage.SetActive(false);
    }
    private void OnEnable()
    {
        rewardPage.SetActive(false);
    }

    public void OnClickOn()
    {
        rewardPage.SetActive(true);
    }
    public void OnClickOff()
    {
        rewardPage.SetActive(false);
    }
    public void AddGold(string count)
    {
        var gold = GameManager.Instance.itemInfoList.Find(t => t["ID"].ToString().Equals(goldId));
        string name = gold["Item_Name"].ToString();
        string info = gold["Info"].ToString();
        string iconName = gold["Icon_Name"].ToString();
        string sort = gold["Sort"].ToString();
        string dataId = gold["DataID"].ToString();
        AddItem(goldId, name, iconName, info, dataId,sort, count, false);
    }
    public void AddItem(string id, string name, string iconName, string info,string sort, string dataId, string count, bool isEventReward)
    {
        if (isEventReward)
        {
            var currReward = Instantiate(item, eventRewardTr);
            currReward.SetData(id, name, iconName, info, sort, dataId, count);
            currRewards.Add(currReward);
        }

        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].data.id.CompareTo(id) == 0)
            {
                rewards[i].AddCount(count);
                return;
            }
        }

        var newItem = Instantiate(item, rewardTr);
        newItem.SetData(id, name, iconName, info, sort, dataId, count);

        rewards.Add(newItem);
    }

    public void Sort(RewardItem item)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (item.data.id.CompareTo(rewards[i].data.id) < 0)
            {
                item.transform.SetSiblingIndex(i);
                return;
            }
        }
    }

    public void SortAll()
    {
        for (int i = 0; i < rewards.Count - 1; i++)
        {
            for (int j = i + 1; j < rewards.Count; j++)
            {
                if (rewards[i].data.id.CompareTo(rewards[j].data.id) < 0)
                {
                    var temp = rewards[i];
                    rewards[i] = rewards[j];
                    rewards[j] = temp;
                }
            }
        }

        // 각각의 아이템에 SetSiblingIndex를 호출해서 인덱스를 설정해줌
        for (int i = 0; i < rewards.Count; i++)
        {
            rewards[i].transform.SetSiblingIndex(i);
        }
    }

    /***************** Minu *****************/
    public void OnEventRewardPage()
    {
        eventRewardPage.SetActive(true);
    }
    public void OffEventRewardPage()
    {
        DestroyCurrRewards();
        eventRewardPage.SetActive(false);
    }

    public void DestroyCurrRewards()
    {
        for (int i = 0; i < currRewards.Count; i++)
        {
            Destroy(currRewards[i].gameObject);
        }
        currRewards.Clear();
    }
}
