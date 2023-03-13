using System.Collections.Generic;
using UnityEngine;

public class BattleMapInfo : MonoBehaviour
{
    public List<Transform> startPositions;
    public List<Transform> endPositions;

    public List<MapEventTrigger> triggers;

    public Transform roadTr;
    protected BattleManager evManager;
    public GameObject platform;
    public BattleMapEnum battleMapType;
    public GameObject viewPoint;

    protected void FindEvManager()
    {
        evManager = FindObjectOfType<BattleManager>();
    }

    public List<Transform> GetStartPosition()
    {
        return startPositions;
    }
    public List<Transform> GetEndPosition()
    {
        return endPositions;
    }

    public GameObject GetViewPoint()
    {
        return viewPoint;
    }

    public int GetAllEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < triggers.Count; i++)
        {
            count += triggers[i].enemys.Count;
        }

        return count;
    }

    public int GetCurrEnemyCount()
    {
        return triggers[evManager.GetCurrTriggerIndex()].useEnemys.Count;
    }

    public void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[evManager.GetCurrTriggerIndex()].useEnemys;
    }

    public ref List<MapEventTrigger> GetTriggers()
    {
        return ref triggers;
    }
    public List<Transform> GetStartPositions()
    {
        return startPositions;
    }
    public Transform GetRoadTr()
    {
        return roadTr;
    }
    public GameObject GetPlatform()
    {
        return platform;
    }
    public MapEventTrigger GetTrigger(int index)
    {
        return triggers[index];
    }
    public BattleMapEnum GetBattleMapType()
    {
        return battleMapType;
    }
}
