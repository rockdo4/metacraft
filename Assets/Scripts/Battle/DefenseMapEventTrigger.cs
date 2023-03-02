using UnityEngine;

public class DefenseMapEventTrigger : MapEventTrigger
{
    public TestBattleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
        AddUseEnemyList(enemy);
        enemys.Remove(enemy);
        manager.EnemyTriggerEnter();
    }
}
