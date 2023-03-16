using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReward : MonoBehaviour
{
    public GameObject rewardPage;

    public RewardItem item;
    public Transform rewardTr;
    public List<RewardItem> items;

    private void OnDisable()
    {
        rewardPage.SetActive(false);
    }
    private void OnEnable()
    {
        rewardPage.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Insert))
        {
            AddItem(0, "Test0", "5");
        }
        if (Input.GetKeyDown(KeyCode.Home))
        {
            AddItem(1, "Test1", "5");
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            AddItem(2, "Test2", "5");
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SortAll();
        }
    }

    public void OnClickOn()
    {
        rewardPage.SetActive(true);
    }
    public void OnClickOff()
    {
        rewardPage.SetActive(false);
    }

    public void AddItem(int id, string text, string count)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                items[i].AddCount(count);
                return;
            }
        }

        var newItem = Instantiate(item, rewardTr);
        newItem.SetData(id, text, count);
        items.Add(newItem);
    }

    public void Sort(RewardItem item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (item.Id < items[i].Id)
            {
                item.transform.SetSiblingIndex(i);
                return;
            }
        }
    }

    public void SortAll()
    {
        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                if (items[i].Id > items[j].Id)
                {
                    var temp = items[i];
                    items[i] = items[j];
                    items[j] = temp;
                }
            }
        }

        // 각각의 아이템에 SetSiblingIndex를 호출해서 인덱스를 설정해줌
        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.SetSiblingIndex(i);
        }
    }
}
