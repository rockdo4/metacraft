using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public List<BuffInfo> buffList;
    Dictionary<int, BuffInfo> allBuff = new();

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;

            foreach (var buff in buffList)
            {
                allBuff[buff.id] = buff;
            }
        }
    }

    public BuffInfo GetBuff(int id) => allBuff[id];
}