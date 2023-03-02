using System;
using System.Collections.Generic;
using UnityEngine;

public class ManageDispatchWindow : View
{
    public GameObject dispatchPrefab;
    public Transform[] contents;
    private List<Dictionary<string, object>> dispatchInfoTable;

    public List<DispatchInfo> goldDispatches = new();
    public List<DispatchInfo> advancementMaterialDispatches = new();
    public List<DispatchInfo> skillMaterialDispatches = new();


    private void Start()
    {
        dispatchInfoTable = GameManager.Instance.dispatchInfoList;
        for (int i = 2; i < dispatchInfoTable.Count; i++)
        {
            switch (dispatchInfoTable[i]["Type"])
            {
                case 0: //재화
                    {
                        GameObject obj = Instantiate(dispatchPrefab, contents[0]);
                        DispatchInfo info = obj.GetComponent<DispatchInfo>();
                        info.SetData(dispatchInfoTable[i]);
                        goldDispatches.Add(info);
                        break;
                    }
                case 1: //승급재료
                    {
                        GameObject obj = Instantiate(dispatchPrefab, contents[1]);
                        DispatchInfo info = obj.GetComponent<DispatchInfo>();
                        info.SetData(dispatchInfoTable[i]);
                        advancementMaterialDispatches.Add(info);
                        break;
                    }
                case 2: //스킬재료
                    {
                        GameObject obj = Instantiate(dispatchPrefab, contents[2]);
                        DispatchInfo info = obj.GetComponent<DispatchInfo>();
                        info.SetData(dispatchInfoTable[i]);
                        skillMaterialDispatches.Add(info);
                        break;
                    }
            }
        }
    }

    private void SetInfos()
    {
        int count = 0;
        foreach (var dispatchInfo in dispatchInfoTable)
        {
            goldDispatches[count].SetData(dispatchInfo);
            goldDispatches[count].gameObject.SetActive(true);
            count++;
        }
    }
}