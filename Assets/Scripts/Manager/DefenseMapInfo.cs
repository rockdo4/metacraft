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

        // 강적 소환
        var enemy = Instantiate(battleMgr.middleBoss);
        enemy.SetEnabledPathFind(false);

        int posCount = triggers[enemyTriggerIndex].enemySettingPositions.Count - 1;
        int randomIndex = Random.Range(0, posCount);
        triggers[enemyTriggerIndex].
            enemySettingPositions[randomIndex].
            SpawnAllEnemy(ref triggers[enemyTriggerIndex].enemys, enemy, 0);

        enemy.SetEnabledPathFind(true);
        enemy.ChangeUnitState(UnitState.Battle);
        triggers[enemyTriggerIndex].enemySettingPositions[randomIndex].isMiddleBoss = true;
        triggers[enemyTriggerIndex].enemySettingPositions[randomIndex].middleBoss = enemy;
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[enemyTriggerIndex].useEnemys;
    }
}
