using System.Collections.Generic;
using UnityEngine;

public class DefenseBattleManager : TestBattleManager
{
    private int enemyCount;

    private void Start()
    {
        GameStart();
        enemyCount = GetAllEnemyCount();
        enemyCountTxt.Count = enemyCount;
    }

    public override void OnDeadHero(AttackableHero hero)
    {
        base.OnDeadHero(hero);
    }

    public override void OnDeadEnemy(AttackableEnemy enemy)
    {
        base.OnDeadEnemy(enemy);
        triggers[0].OnDead(enemy);

        enemyCount--;
        if (enemyCount == 0)
        {
            SetHeroReturnPositioning(startPositions);
        }
    }
    public override void GetEnemyList(ref List<AttackableEnemy> enemyList) 
    {
        enemyList = triggers[0].useEnemys;
    }
    public override void OnReady()
    {
        readyCount--;
        if (readyCount == 0 && enemyCount == 0)
        {
            if (triggers[0].isMissionEnd)
            {
                SetStageClear();
            }
            else
            {
                // test
                SetStageClear();
            }
        }
    }
    protected override void SetHeroReturnPositioning(List<Transform> pos)
    {
        base.SetHeroReturnPositioning(pos);
    }
    private void GameStart()
    {
        triggers[0].SpawnAllEnemy();
        triggers[0].InfinityRespawnEnemy();
    }
}
