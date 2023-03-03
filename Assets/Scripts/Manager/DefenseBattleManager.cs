using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBattleManager : TestBattleManager
{
    private int enemyCount;

    private void Start()
    {
        GameStart();
        enemyCount = GetAllEnemyCount();
        Logger.Debug(enemyCount);
        enemyCountTxt.Count = enemyCount;
    }

    public override void OnDeadHero(AttackableHero hero)
    {
        base.OnDeadHero(hero);
        readyCount = useHeroes.Count;
        if (useHeroes.Count == 0)
        {
           // SetEnemyIdle();
        }
    }
    public override void OnDeadEnemy(AttackableEnemy enemy)
    {
        base.OnDeadEnemy(enemy);
        triggers[0].OnDead(enemy);

        enemyCount--;
        Logger.Debug($"{triggers[0].enemys.Count} / {triggers[0].useEnemys.Count} / {enemyCount}");
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
        Logger.Debug(readyCount);
        Logger.Debug(enemyCount);
        if (readyCount == 0 && enemyCount == 0)
        {
            SetStageClear();
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
