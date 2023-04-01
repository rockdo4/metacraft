using UnityEngine;

public class DefenseMapEventTrigger : MapEventTrigger
{
    public BattleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemyTrigger && other.CompareTag("Enemy"))
        {
            AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
            AddUseEnemyList(enemy);
            enemys.Remove(enemy);
            manager.EnemyTriggerEnter();
        }

        if (!isTriggerEnter)
        {
            if (other.CompareTag("Hero"))
            {
                isTriggerEnter = true;
            }
        }
    }
}
