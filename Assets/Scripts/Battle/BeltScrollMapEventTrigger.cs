using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BeltScrollMapEventTrigger : MapEventTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        if (!isLastTrigger && other.TryGetComponent<AttackableHero>(out var hero))
            hero.ChangeUnitState(UnitState.Battle);

        if (!isTriggerEnter)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].SetEnabledPathFind(true);
                enemys[i].ChangeUnitState(UnitState.Battle);
                AddUseEnemyList(enemys[i]);
            }
            enemys.Clear();

            isTriggerEnter = true;
        }
    }
}
