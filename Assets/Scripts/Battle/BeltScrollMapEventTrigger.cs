using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltScrollMapEventTrigger : MapEventTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        var hero = other.GetComponent<AttackableHero>();
        hero.ChangeUnitState(UnitState.Battle);

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].SetEnabledPathFind(true);
            enemys[i].ChangeUnitState(UnitState.Battle);
            AddUseEnemyList(enemys[i]);
        }
        enemys.Clear();
    }
}
