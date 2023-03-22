using UnityEngine;

public class DefenseMapEventTrigger : MapEventTrigger
{
    public BattleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggerEnter)
        {
            if (other.CompareTag("Enemy") && isEnemyTrigger)
            {
                AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
                AddUseEnemyList(enemy);
                enemys.Remove(enemy);
                manager.EnemyTriggerEnter();
            }
            else if (other.CompareTag("Hero"))
            {
                isTriggerEnter = true;
            }
        }
    }
}
