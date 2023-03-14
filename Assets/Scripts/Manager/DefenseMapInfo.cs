public class DefenseMapInfo : BattleMapInfo
{
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

        SettingMap(index);
        battleMgr.SetEnemyTriggerIndex(index);
    }

    private void SettingMap(int index)
    {
        if (!init)
        {
            triggers[index].SpawnAllEnemy();
            init = true;
        }

        triggers[index].InfinityRespawnEnemy();
    }
}
