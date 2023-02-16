using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public const int latestVersion = 1;

    private int selectIndex = 0;
    private int saveFileLimit = 12;
    private readonly string fileName = "save";
    private readonly string extension = ".json";

    public List<SaveData> saveDatas = new();

    private void Start()
    {
        saveDatas = new(saveFileLimit);
        LoadAllFiles();
        PrintState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Test Generate
            Dictionary<int, int> range = NormalDistribution.GetRange(10000, 1, 10).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            
            //foreach (var item in range)
            //    Logger.Debug($"{item.Key}:{item.Value}");

            Vector3 pos = new (Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
            Quaternion rot = new (Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
            (saveDatas[selectIndex] as SaveDataV01).SetMembers(range.Keys.ToList(), range.Values.ToList(), pos, rot);
            Logger.Debug("Generate");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveToJson(selectIndex);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadToJson(selectIndex);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectIndex < saveFileLimit)
                selectIndex++;
            Logger.Debug($"{selectIndex}:00");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectIndex > 0)
                selectIndex--;
            Logger.Debug($"{selectIndex}:00");
        }
    }

    public void LoadAllFiles()
    {
        for (int i = 0; i < saveFileLimit; i++)
        {
            saveDatas.Add(LoadToJson(i));
        }
    }

    public void PrintState()
    {
        string temp = string.Empty;
        for (int i = 0; i < saveFileLimit; i++)
        {
            temp += (saveDatas[i] == null ? "0" : "1");

            if (i == (saveFileLimit / 2))
                temp += "\n";
        }
        Logger.Debug(temp);
    }

    public void SaveToJson(int idx)
    {
        File.WriteAllText(MakeFilePath(idx), saveDatas[idx].GetJson(true));
        Logger.Debug("Save");
    }

    public SaveData LoadToJson(int idx)
    {
        string path = MakeFilePath(idx);
        if (!File.Exists(path))
        {
            Logger.Debug("Load Failed");
            return null;
        }
        string jsonFromFile = File.ReadAllText(path);
        Logger.Debug("Load");
        return JsonUtility.FromJson<SaveDataV01>(jsonFromFile);
    }

    private string MakeFileName(int curIdx)
    {
        return $"{fileName}{curIdx:00}{extension}";
    }

    private string MakeFilePath(int curIdx)
    {
        return $"{Application.dataPath}/Tables/Saves/{MakeFileName(curIdx)}";
    }
}