using System.Collections.Generic;

public class DefenseMapInfo : BattleMapInfo
{
    private int enemyTriggetIndex;

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

        enemyTriggetIndex = index;
        SettingMap(enemyTriggetIndex);
        battleMgr.SetEnemyTriggerIndex(enemyTriggetIndex);
        int count = GetAllEnemyCount();
        battleMgr.SetEnemyCountTxt(count);
    }

    private void SettingMap(int index)
    {
        if (!init)
        {
            triggers[index].SpawnAllEnemy();
            init = true;
        }
        else
        {
            triggers[index].ResetEnemys();
            triggers[index].SetEnemysActive(false);
        }

        triggers[index].InfinityRespawnEnemy();
        triggers[index].isSkip = true;
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[enemyTriggetIndex].useEnemys;
    }
}
