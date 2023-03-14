using UnityEngine;

public class DefenseMapEventTrigger : MapEventTrigger
{
    public BattleManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AttackableHero>(out var hero))
            hero.ChangeUnitState(UnitState.Battle);

        if (other.CompareTag("Enemy"))
        {
            AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
            AddUseEnemyList(enemy);
            enemys.Remove(enemy);
            manager.EnemyTriggerEnter();
        }
    }
}
