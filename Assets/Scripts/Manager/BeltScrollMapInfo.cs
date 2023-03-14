using System.Collections.Generic;
using UnityEngine;

public class BeltScrollMapInfo : BattleMapInfo
{
    public override void GameStart()
    {
        base.GameStart();
        battleMapType = BattleMapEnum.Normal;
        SettingMap();
        int count = GetAllEnemyCount();
        battleMgr.SetEnemyCountTxt(count);
    }

    private void SettingMap()
    {
        if (!init)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                for (int j = 0; j < triggers[i].enemySettingPositions.Count; j++)
                {
                    var enemy = triggers[i].enemySettingPositions[j]?.SpawnEnemy();
                    if (enemy == null)
                        break;

                    triggers[i].enemys.Add(enemy);
                    triggers[i].enemys[j].SetPathFind();
                    triggers[i].AddEnemyColliders(enemy.GetComponent<CapsuleCollider>());
                    triggers[i].enemys[j].SetEnabledPathFind(false);
                }
            }

            init = true;
        }
        else
        {
            ResetAllTriggerEnemys();
        }
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[battleMgr.GetCurrTriggerIndex()].useEnemys;
    }
}
