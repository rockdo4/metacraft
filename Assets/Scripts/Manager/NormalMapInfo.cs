using System.Collections.Generic;
using UnityEngine;

public class NormalMapInfo : BattleMapInfo
{
    public override void GameStart()
    {
        base.GameStart();
        battleMapType = BattleMapEnum.Normal;
        AllTriggerEnterReset();
        AllEnemyActive(true);
        int count = GetAllEnemyCount();
        battleMgr.SetEnemyCountTxt(count);
    }

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
    {
        enemyList = triggers[battleMgr.GetCurrTriggerIndex()].useEnemys;
    }
}
