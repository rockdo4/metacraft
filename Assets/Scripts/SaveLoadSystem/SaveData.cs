using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SaveData
{
    public int version = 0;

    public abstract string GetJson(bool prettyPrint);
}

public class SaveDataV01 : SaveData
{
    public SaveDataV01()
    {
        version = 1;
    }

    public List<int> keys;
    public List<int> values;
    public Vector3 pos;
    public Quaternion rot;

    public void SetMembers(List<int> keys, List<int> values, Vector3 pos, Quaternion rot)
    {
        this.keys = keys;
        this.values = values;
        this.pos = pos;
        this.rot = rot;
    }

    public Dictionary<int, int> GetDictionary()
    {
        return Utils.ListToDictionary(keys, values);
    }

    public override string GetJson(bool prettyPrint = false)
    {
        return JsonUtility.ToJson(this, prettyPrint);
    }
}
