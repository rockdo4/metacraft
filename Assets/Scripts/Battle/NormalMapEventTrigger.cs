using UnityEngine;

public class NormalMapEventTrigger : MapEventTrigger
{
    private void OnTriggerEnter(Collider other)
    {
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
