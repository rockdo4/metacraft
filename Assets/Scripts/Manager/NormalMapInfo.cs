using System.Collections.Generic;
using UnityEngine;

public class NormalMapInfo : BattleMapInfo
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
                    // 각 위치마다 데이터 설정 (적 프리펩, 데이터들)

                    var enemy = triggers[i].enemySettingPositions[j]?.SpawnEnemy();
                    if (enemy == null)
                        break;

                    for (int k = 0; k < enemy.Count; k++)
                    {
                        triggers[i].enemys.Add(enemy[k]);
                        triggers[i].enemys[j].SetPathFind();
                        triggers[i].AddEnemyColliders(enemy[k].GetComponent<CapsuleCollider>());
                        triggers[i].enemys[j].SetEnabledPathFind(false);
                    }
                }
            }

            init = true;
        }
        else
        {
            ResetAllTriggerEnemys();
            AllTriggerEnterReset();
        }
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[battleMgr.GetCurrTriggerIndex()].useEnemys;
    }
}
