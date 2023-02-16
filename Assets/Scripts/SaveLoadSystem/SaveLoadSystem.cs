using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public const int latestVersion = 1;

    private int selectIndex = 0;
    private readonly string fileName = "save";
    private readonly string extension = ".json";

    Dictionary<string, SaveData> saveDatas = new();
    SaveData saveData = new SaveDataV1();

    private string MakeFileName(int curIdx)
    {
        return $"{fileName}{curIdx:00}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Test Generate
            //SaveDataV1 sd1 = saveData as SaveDataV1;
            (saveData as SaveDataV1).range = NormalDistribution.GetRange(10000, 1, 100);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveToJson(selectIndex);
            selectIndex++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Load
        }
    }

    public void SaveToJson(int idx)
    {

        string json = JsonUtility.ToJson(saveData, true);
        Logger.Debug(json);
    }

    public void LoadToJson(int idx)
    {
        JsonUtility.FromJson<SaveData>($"{MakeFileName(idx)}{extension}");
    }
}