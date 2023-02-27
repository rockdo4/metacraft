using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBattleManager : TestBattleManager
{
    private void Start()
    {
        GameStart();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }

    private void Update()
    {

    }
    public override void OnDeadHero(AttackableHero hero)
    {
        base.OnDeadHero(hero);
    }
    public override void OnDeadEnemy(AttackableEnemy enemy)
    {
        base.OnDeadEnemy(enemy);
        triggers[0].OnDead(enemy);
    }
    public override void GetEnemyList(ref List<AttackableEnemy> enemyList) 
    {
        enemyList = triggers[0].useEnemys;
    }
    public override void OnReady()
    {
        base.OnReady();
    }
    private void GameStart()
    {
        triggers[0].TestInfinityRespawnEnemy();
    }
}
