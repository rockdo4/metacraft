using UnityEngine;
using System.Collections.Generic;


public class DefenseMapInfo : BattleMapInfo
{
    private int enemyTriggerIndex;

    public override void GameStart()
    {
        base.GameStart();
        battleMapType = BattleMapEnum.Defense;

        int index = 0;
        for (int i = 0; i < triggers.Count; i++)
        {
            if (triggers[i].enemySettingPositions.Count > 0)
            {
                index = i;
                break;
            }
        }

        enemyTriggerIndex = index;
        SettingMap(enemyTriggerIndex);
        battleMgr.SetEnemyTriggerIndex(enemyTriggerIndex);
        int count = GetAllEnemyCount();
        battleMgr.SetEnemyCountTxt(count);
    }

    private void SettingMap(int index)
    {
        triggers[index].isEnemyTrigger = true;
        triggers[index].InfinityRespawnEnemy();
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[enemyTriggerIndex].useEnemys;
    }
}
