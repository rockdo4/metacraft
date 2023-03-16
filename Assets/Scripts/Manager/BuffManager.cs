using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            List<AttackableUnit> players = new();
            GameObject.FindObjectOfType<BattleManager>().GetHeroList(ref players);

            players[0].AddStateBuff(GetBuff(8));
        }
    }
}

