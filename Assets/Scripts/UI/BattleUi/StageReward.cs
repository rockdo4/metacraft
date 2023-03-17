using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReward : MonoBehaviour
{
    public GameObject rewardPage;

    public RewardItem item;
    public Transform rewardTr;
    public List<RewardItem> rewards;
    public int golds = 0;
    public List<Dictionary<string, object>> itemInfoList; // 아이템 정보

    public string goldId;

    private void Start()
    {
        itemInfoList = GameManager.Instance.itemInfoList;
    }

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
    public void AddGold(string value)
    {
        AddItem(goldId, value);
    }
    public void AddItem(string id, string count)
    {

        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].Id.CompareTo(id) == 0)
            {
                rewards[i].AddCount(count);
                return;
            }
        }

        var newItem = Instantiate(item, rewardTr);
        newItem.SetData(id, "", count);
        rewards.Add(newItem);
    }

    public void Sort(RewardItem item)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            if (item.Id.CompareTo(rewards[i].Id) < 0)
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
                if (rewards[i].Id.CompareTo(rewards[j].Id) < 0)
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
}
