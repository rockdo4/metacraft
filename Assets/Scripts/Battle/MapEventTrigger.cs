using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public TestBattleManager manager;
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ�� �־��ּ���.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys = new();        // ������ Enemy��
    public List<AttackableEnemy> useEnemys = new();     // ���� ������ Enemy��

    public GameObject enemyPool; // Enemy GameObject���� �� �θ�

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AttackableHero>() != null)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].SetEnabledPathFind(true);
                enemys[i].ChangeUnitState(UnitState.Battle);
                AddUseEnemyList(enemys[i]);
            }
            enemys.Clear();
        }
        else
        {
            AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
            AddUseEnemyList(enemy);
            enemys.Remove(enemy);
            manager.EnemyTriggerEnter();
        }
    }

    public void OnDead(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
        enemy.gameObject.SetActive(false);
        enemys.Add(enemy);
    }

    public void InfinityRespawnEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            float timer = Random.Range(1f, 2f);
            enemySettingPositions[i].InfinityRespawn(timer);
        }
    }

    public void AddUseEnemyList(AttackableEnemy enemy)
    {
        useEnemys.Add(enemy);
    }
    public void SubUseEnemyList(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
    }
    public void SpawnAllEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].SpawnAllEnemy(ref enemys);
        }

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(false);
        }
    }
}
