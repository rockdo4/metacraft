using System.Collections.Generic;
using UnityEngine;

public class DefenseBattleManager : TestBattleManager
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            triggers[0].TestRespawnAllEnemy();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            for (int i = 0; i < triggers[0].enemys.Count; i++)
            {
                triggers[0].enemys[i].ChangeUnitState(UnitState.Battle);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].ChangeUnitState(UnitState.Battle);
            }
        }
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
        // Test
        enemyList = triggers[0].enemys;
    }
    public override void OnReady()
    {
        base.OnReady();
    }
}
