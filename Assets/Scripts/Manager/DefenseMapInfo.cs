using System.Collections.Generic;
using UnityEngine;

public class DefenseMapInfo : BattleMapInfo
{
    private void Start()
    {
        battleMapType = BattleMapEnum.Defense;
        GameStart();
    }

    private void GameStart()
    {
        triggers[0].SpawnAllEnemy();
        triggers[0].InfinityRespawnEnemy();
    }
}
